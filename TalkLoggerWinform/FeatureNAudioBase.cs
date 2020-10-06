// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tono;
using Tono.GuiWinForm;
using static TalkLoggerWinform.CoreFeatureBase;

namespace TalkLoggerWinform
{
    /// <summary>
    /// Capture voice with NAudio
    /// </summary>
    public abstract class FeatureNAudioBase : FeatureAudioCaptureBase, IMultiTokenListener, ICloseCallback
    {
        public NamedId[] MultiTokenTriggerID { get; } = new NamedId[] { FeaturePlayButton.TokenStart, FeaturePlayButton.TokenStop };

        private readonly Queue<SpeechHandler> _handlers = new Queue<SpeechHandler>();
        private string _talkID = null;

        public override void OnInitInstance()
        {
            base.OnInitInstance();

            Finalizers.Add(() =>
            {
                Hot.AddWavToAllQueue(DisplayName, null, 0); // Prepqre buffer of DisplayName
            });
        }
        public override void Start(NamedId who)
        {
            base.Start(who);

            if (FeaturePlayButton.TokenStart.Equals(who))
            {
                _ = Setup(new SpeechHandler()); // Uncontroll thread
            }
            if (FeaturePlayButton.TokenStop.Equals(who))
            {
                var hs = _handlers.ToArray();
                _handlers.Clear();
                Task.Run(() =>
                {
                    Reset(hs);
                });
            }
        }

        private void Reset(SpeechHandler[] handlers)
        {
            _talkID = null;
            foreach (var handler in handlers)
            {
                LOG.WriteLine(LLV.DEV, $"☆STOPPING {GetType().Name} instance {handler.TimeGenerated.ToString(TimeUtil.FormatYMDHMSms)}");
                Application.DoEvents();
                handler?.FireStop();    // STOP NAudio device
                handler?.Recognizer?.Dispose(); // STOP Azure Cognitive Service // await handler?.Recognizer?.StopContinuousRecognitionAsync();  
                LOG.WriteLine(LLV.DEV, $"★STOPPED {GetType().Name} instance {handler.TimeGenerated.ToString(TimeUtil.FormatYMDHMSms)}");
            }
        }


        private async Task Setup(SpeechHandler handler)
        {
            _handlers.Enqueue(handler);

            var taskList = new Func<SpeechHandler, Task<bool>>[] {
                            SelectAudioDeviceAsync,
                            MakeAudioConfigAsync,
                            StartRecognizeSpeechAsync,
            };
            foreach (var func in taskList)
            {
                var ret = await func.Invoke(handler);
                if (!ret)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Thread safe handler
        /// </summary>
        private class SpeechHandler
        {
            public DateTime TimeGenerated { get; } = DateTime.Now;
            public MMDevice Device { get; set; }
            public SpeechRecognizer Recognizer { get; set; }
            public PushAudioInputStream AudioInputStream { get; set; }
            public AudioConfig AudioConfig { get; set; }

            public event EventHandler StopRequested;

            public void FireStop()
            {
                StopRequested?.Invoke(null, EventArgs.Empty);
            }
        }

        protected virtual MMDeviceCollection GetDeviceEndpoints()
        {
            throw new NotSupportedException();
        }

        protected virtual string GetTargetDeviceID()
        {
            throw new NotSupportedException();
        }

        protected virtual string GetTargetRecognizeLanguage()
        {
            throw new NotSupportedException();
        }

        private async Task<bool> SelectAudioDeviceAsync(SpeechHandler handler)
        {
            // SELECT A AUDIO DEVICE
            var devices = GetDeviceEndpoints();
            foreach (var device in devices)
            {
                if (device.ID == GetTargetDeviceID())
                {
                    handler.Device = device;
                    LOG.WriteMesLine(GetType().Name, "DeviceInitialized", device.FriendlyName);
                    return true;
                }
            }
            LOG.WriteMesLine(GetType().Name, "NoDeviceID", Hot.Setting.Device2ID);
            await Task.Delay(20);
            return false;
        }

        protected virtual IWaveIn CreateCaptureInstance(MMDevice device)
        {
            throw new NotSupportedException();
        }

        private async Task<bool> MakeAudioConfigAsync(SpeechHandler handler)
        {
            Debug.Assert(handler.Device != null);

            var lastSpeakDT = DateTime.Now;
            var willStop = DateTime.MaxValue;

            // NAudio Setting
            var wavein = CreateCaptureInstance(handler.Device);
            var waveoutFormat = WaveFormat.CreateIeeeFloatWaveFormat(16000, 1); //new WaveFormat(16000, 16, 1);
            wavein.StartRecording();


            // Azure Cognitive Service Setting
            var audioformat = AudioStreamFormat.GetWaveFormatPCM((uint)waveoutFormat.SampleRate, (byte)waveoutFormat.BitsPerSample, (byte)waveoutFormat.Channels);
            handler.AudioInputStream = AudioInputStream.CreatePushStream(audioformat);
            handler.AudioConfig = AudioConfig.FromStreamInput(handler.AudioInputStream);

            // NAudio Voice event
            wavein.DataAvailable += (s, e) =>
            {
                if (e.BytesRecorded > 0)
                {
                    // Hot.SetWavFormat(DisplayName, wavein.WaveFormat);                // Clear Sound
                    // Hot.AddWavToAllQueue(DisplayName, e.Buffer, e.BytesRecorded);    // Clear Sound

                    using (var ms = new MemoryStream(e.Buffer, 0, e.BytesRecorded))
                    using (var rs = new RawSourceWaveStream(ms, wavein.WaveFormat))
                    using (var freq = new MediaFoundationResampler(rs, waveoutFormat))
                    {
                        Hot.SetWavFormat(DisplayName, freq.WaveFormat);   // TESTING

                        var buf = new byte[512];
                        for( var len = 1; len != 0; )
                        {
                            len = freq.Read(buf, 0, buf.Length);
                            if( len > 0)
                            {
                                handler.AudioInputStream.Write(buf, len);       // for Azure Cognitive Speech to Text
                                Hot.AddWavToAllQueue(DisplayName, buf, len);    // for File Saving
                            }
                        }

                        lastSpeakDT = DateTime.Now;
                        willStop = DateTime.MaxValue;

                    }
                    Token.Add(TokenWavDataQueued, this);    // TODO: Need Confirm it must be fixed with Tono.Gui.WinForm 1.1.2 - System.InvalidOperationException: 'Collection was modified; enumeration operation may not execute.'
                }
                else
                {
                    if (DateTime.Now < willStop)
                    {
                        if (willStop == DateTime.MaxValue)
                        {
                            willStop = DateTime.Now + TimeSpan.FromSeconds(10);
                        }
                        var silence = new SilenceProvider(waveoutFormat);
                        var buf = new byte[waveoutFormat.BitsPerSample * waveoutFormat.SampleRate / 8 / 100];       // 10ms
                        var len = silence.Read(buf, 0, buf.Length);
                        var cnt = (int)((DateTime.Now - lastSpeakDT).TotalMilliseconds / 10);
                        for (var i = 0; i < cnt; i++)
                        {
                            handler.AudioInputStream?.Write(buf, len);      // for Azure Cognitive Speech to Text
                            //Hot.AddWavToAllQueue(DisplayName, handler.buf, len);    // for File Saving
                        }
                        lastSpeakDT = DateTime.Now;
                        //Token.Add(TokenWavDataQueued, this);  // for File Saving
                    }
                }
            };

            handler.StopRequested += (s, e) =>
            {
                wavein.StopRecording();     // Stop NAudio recording
            };

            return await Task.FromResult(true);
        }
        private async Task<bool> StartRecognizeSpeechAsync(SpeechHandler handler)
        {
            handler.Recognizer = new SpeechRecognizer(SpeechConfig.FromSubscription(Hot.Setting.SubscriptionKey, Hot.Setting.ServiceRegion), GetTargetRecognizeLanguage(), handler.AudioConfig);
            handler.Recognizer.Recognizing += OnRecognizing;
            handler.Recognizer.Recognized += OnRecognized;
            handler.Recognizer.Canceled += OnCancel;
            handler.Recognizer.SessionStarted += OnSessionStarted;
            handler.Recognizer.SessionStopped += OnSessionStopped;
            handler.Recognizer.SpeechStartDetected += OnSpeechStartDetected;
            handler.Recognizer.SpeechEndDetected += OnSpeechEndDetected;

            await handler.Recognizer.StartContinuousRecognitionAsync();

            return true;
        }

        private void OnSpeechStartDetected(object sender, RecognitionEventArgs e)
        {
            LOG.NoJumpNext();
            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}({GetTargetRecognizeLanguage()}).OnSpeechStartDetected : {e}");
        }
        private void OnSpeechEndDetected(object sender, RecognitionEventArgs e)
        {
            LOG.NoJumpNext();
            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}({GetTargetRecognizeLanguage()}).OnSpeechEndDetected : {e}");
        }

        private void OnSessionStarted(object sender, SessionEventArgs e)
        {
            LOG.NoJumpNext();
            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}({GetTargetRecognizeLanguage()}).OnSessionStarted : {e}");
        }

        private void OnSessionStopped(object sender, SessionEventArgs e)
        {
            LOG.NoJumpNext();
            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}({GetTargetRecognizeLanguage()}).OnSessionStopped : {e}");
        }

        private void OnRecognizing(object sender, SpeechRecognitionEventArgs e)
        {
            LOG.NoJumpNext();
            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}({GetTargetRecognizeLanguage()}).OnRecognizing : {e.Result.Text}");

            if (_talkID == null)
            {
                _talkID = Guid.NewGuid().ToString();
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent
                {
                    RowID = ID.Value,
                    Action = SpeechEvent.Actions.Start,
                    TimeGenerated = DateTime.Now,
                    SessionID = _talkID,
                });
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent
                {
                    RowID = ID.Value,
                    Action = SpeechEvent.Actions.SetColor,
                    TimeGenerated = DateTime.Now,
                    SessionID = _talkID,
                    Text = GetBarColor().ToArgb().ToString(),
                });
            }

            Hot.SpeechEventQueue.Enqueue(new SpeechEvent
            {
                RowID = ID.Value,
                Action = SpeechEvent.Actions.Recognizing,
                TimeGenerated = DateTime.Now,
                SessionID = _talkID,
                Text = e.Result.Text,
            });
            Token.Add(TokenSpeechEventQueued, this);
            GetRoot().FlushFeatureTriggers();
        }

        protected virtual Color GetBarColor()
        {
            throw new NotSupportedException();
        }

        private void OnRecognized(object sender, SpeechRecognitionEventArgs e)
        {
            if (_talkID == null)
                return;

            LOG.NoJumpNext();
            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}({GetTargetRecognizeLanguage()}).OnRecognized : {e.Result.Text}");
            Hot.SpeechEventQueue.Enqueue(new SpeechEvent
            {
                RowID = ID.Value,
                Action = SpeechEvent.Actions.Recognized,
                TimeGenerated = DateTime.Now,
                SessionID = _talkID,
                Text = e.Result.Text,
            });
            _talkID = null;
            Token.Add(TokenSpeechEventQueued, this);
            GetRoot().FlushFeatureTriggers();
        }
        private void OnCancel(object sender, SpeechRecognitionEventArgs e)
        {
            if (_talkID == null)
                return;

            LOG.NoJumpNext();
            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}({GetTargetRecognizeLanguage()}).OnCancel : {e.Result.Text}");
            Hot.SpeechEventQueue.Enqueue(new SpeechEvent
            {
                RowID = ID.Value,
                Action = SpeechEvent.Actions.Canceled,
                TimeGenerated = DateTime.Now,
                SessionID = _talkID,
                Text = e.Result.Text,
            });
            _talkID = null;
            Token.Add(TokenSpeechEventQueued, this);
            GetRoot().FlushFeatureTriggers();
        }

        public void OnClosing()
        {
            var hs = _handlers.ToArray();
            _handlers.Clear();
            Reset(hs);
        }
    }
}
