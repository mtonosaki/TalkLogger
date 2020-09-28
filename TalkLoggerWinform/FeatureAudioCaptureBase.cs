// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    /// <summary>
    /// Capture voice with NAudio
    /// </summary>
    public abstract class FeatureAudioCaptureBase : CoreFeatureBase, IMultiTokenListener
    {
        public NamedId[] MultiTokenTriggerID { get; } = new NamedId[] { FeaturePlayButton.TokenStart, FeaturePlayButton.TokenStop };

        private readonly Queue<SpeechHandler> _handlers = new Queue<SpeechHandler>();
        private string _talkID = null;

        public override void OnInitInstance()
        {
            base.OnInitInstance();

            Pane.Control.FindForm().FormClosing += FeatureAudioLoopback_FormClosing;

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
                Task.Run(() => {
                    Reset(hs);
                });
            }
        }

        private void Reset(SpeechHandler[] handlers)
        {
            _talkID = null;
            foreach( var handler in handlers)
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
        private void FeatureAudioLoopback_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            var hs = _handlers.ToArray();
            _handlers.Clear();
            Reset(hs);
        }

        private class SpeechHandler
        {
            public DateTime TimeGenerated { get; } = DateTime.Now;
            public MMDevice Device { get; set; }
            public SpeechRecognizer Recognizer { get; set; }
            public PushAudioInputStream AudioInputStream { get; set; }
            public AudioConfig AudioConfig { get; set; }
            public byte[] buf = new byte[1024 * 80];

            public event EventHandler StopRequested;

            public void FireStop()
            {
                StopRequested?.Invoke(null, EventArgs.Empty);
            }
        }

        protected virtual MMDeviceCollection GetDeviceEndpoints() => throw new NotSupportedException();

        protected virtual string GetTargetDeviceID() => throw new NotSupportedException();

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

        protected virtual IWaveIn CreateCaptureInstance(MMDevice device) => throw new NotSupportedException();

        private async Task<bool> MakeAudioConfigAsync(SpeechHandler handler)
        {
            Debug.Assert(handler.Device != null);

            var wavein = CreateCaptureInstance(handler.Device);
            var waveoutFormat = new WaveFormat(16000, 16, 1);
            var lastSpeakDT = DateTime.Now;
            var willStop = DateTime.MaxValue;
            wavein.StartRecording();

            var audioformat = AudioStreamFormat.GetWaveFormatPCM(samplesPerSecond: 16000, bitsPerSample: 16, channels: 1);
            handler.AudioInputStream = AudioInputStream.CreatePushStream(audioformat);
            handler.AudioConfig = AudioConfig.FromStreamInput(handler.AudioInputStream);

            wavein.DataAvailable += (s, e) => {
                if (e.BytesRecorded > 0)
                {
                    using (var ms = new MemoryStream(e.Buffer, 0, e.BytesRecorded))
                    using (var rs = new RawSourceWaveStream(ms, wavein.WaveFormat))
                    using (var freq = new MediaFoundationResampler(rs, waveoutFormat.SampleRate))
                    {
                        var w16 = freq.ToSampleProvider().ToMono().ToWaveProvider16();
                        var len = w16.Read(handler.buf, 0, handler.buf.Length);
                        handler.AudioInputStream.Write(handler.buf, len);
                        lastSpeakDT = DateTime.Now;
                        willStop = DateTime.MaxValue;
                    }
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
                        var len = silence.Read(handler.buf, 0, waveoutFormat.BitsPerSample * waveoutFormat.SampleRate / 8 / 100);    // 10ms
                        var cnt = (int)((DateTime.Now - lastSpeakDT).TotalMilliseconds / 10);
                        for (var i = 0; i < cnt; i++)
                        {
                            handler.AudioInputStream?.Write(handler.buf, len);
                        }
                        lastSpeakDT = DateTime.Now;
                    }
                }
            };

            handler.StopRequested += (s, e) => {
                wavein.StopRecording();     // Tono: 1.終了処理（NAudio）
            };

            return await Task.FromResult(true);
        }
        private async Task<bool> StartRecognizeSpeechAsync(SpeechHandler handler)
        {
            handler.Recognizer = new SpeechRecognizer(SpeechConfig.FromSubscription(Hot.Setting.SubscriptionKey, Hot.Setting.ServiceRegion), "ja-JP", handler.AudioConfig);
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
            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}.OnSpeechStartDetected : {e}");
        }
        private void OnSpeechEndDetected(object sender, RecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}.OnSpeechEndDetected : {e}");
        }

        private void OnSessionStarted(object sender, SessionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}.OnSessionStarted : {e}");
        }

        private void OnSessionStopped(object sender, SessionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}.OnSessionStopped : {e}");
        }

        private void OnRecognizing(object sender, SpeechRecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}.OnRecognizing : {e.Result.Text}");

            if (_talkID == null)
            {
                _talkID = Guid.NewGuid().ToString();
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                    RowID = ID.Value,
                    Action = SpeechEvent.Actions.Start,
                    TimeGenerated = DateTime.Now,
                    SessionID = _talkID,
                });
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                    RowID = ID.Value,
                    Action = SpeechEvent.Actions.SetColor,
                    TimeGenerated = DateTime.Now,
                    SessionID = _talkID,
                    Text = GetBarColor().ToArgb().ToString(),
                });
            }

            Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                RowID = ID.Value,
                Action = SpeechEvent.Actions.Recognizing,
                TimeGenerated = DateTime.Now,
                SessionID = _talkID,
                Text = e.Result.Text,
            });
            Token.Add(TokenSpeechEventQueued, this);
            GetRoot().FlushFeatureTriggers();
        }

        protected virtual Color GetBarColor() => throw new NotSupportedException();

        private void OnRecognized(object sender, SpeechRecognitionEventArgs e)
        {
            if (_talkID == null) return;

            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}.OnRecognized : {e.Result.Text}");
            Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
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
            if (_talkID == null) return;

            LOG.WriteLine(LLV.DEV, $"{DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)} {GetType().Name}.OnCancel : {e.Result.Text}");
            Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
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
    }
}
