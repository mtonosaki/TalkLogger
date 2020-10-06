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

            // NAudio Setting
            var wavein = CreateCaptureInstance(handler.Device);
            var waveoutFormat = new WaveFormat(16000, 16, 1);
            wavein.StartRecording();

            // Azure Cognitive Service Setting
            var audioformat = AudioStreamFormat.GetWaveFormatPCM((uint)waveoutFormat.SampleRate, (byte)waveoutFormat.BitsPerSample, (byte)waveoutFormat.Channels);
            handler.AudioInputStream = AudioInputStream.CreatePushStream(audioformat);
            handler.AudioConfig = AudioConfig.FromStreamInput(handler.AudioInputStream);

            // Silence Generate
            DateTime preEvent = DateTime.Now;
            var silenceData = new byte[waveoutFormat.BlockAlign];

            // Appliation Preparation
            Hot.SetWavFormat(DisplayName, waveoutFormat);   // for file saving

            // NAudio Voice event
            wavein.DataAvailable += (s, e) =>
            {
                if (e.BytesRecorded > 0)
                {
                    var now = DateTime.Now;
                    using (var ms = new MemoryStream())
                    {
                        var memoryWriter = new WaveFileWriter(ms, waveoutFormat);
                        ms.SetLength(0);    // Delete file header.

                        var samples = Resample(wavein.WaveFormat, e.Buffer, e.BytesRecorded, waveoutFormat);
                        foreach (var sample in samples)
                        {
                            memoryWriter.WriteSample(sample);
                        }
                        Hot.AddWavToAllQueue(DisplayName, ms.GetBuffer(), (int)ms.Length, now); // for file saving
                        handler.AudioInputStream.Write(ms.GetBuffer(), (int)ms.Length);         // for Azure Cognitive Speech to Text
                    }
                    Token.Add(TokenWavDataQueued, this);    // TODO: Need Confirm it must be fixed with Tono.Gui.WinForm 1.1.2 - System.InvalidOperationException: 'Collection was modified; enumeration operation may not execute.'
                    preEvent = DateTime.Now;
                }
                else
                {
                    if (_talkID != null)
                    {
                        var spms = (double)waveoutFormat.SampleRate / 1000; // samples per ms
                        var n = (int)(spms * (DateTime.Now - preEvent).TotalMilliseconds);

                        for (var i = n; i >= 0; i--)
                        {
                            handler.AudioInputStream.Write(silenceData, silenceData.Length);    // send silence to azure to get realtime event (othewise, azure will wait untile next event timing even if there is no event long time)
                        }
                    }
                    preEvent = DateTime.Now;
                }
            };

            handler.StopRequested += (s, e) =>
            {
                wavein.StopRecording();     // Stop NAudio recording
            };

            return await Task.FromResult(true);
        }

        private static IEnumerable<float> Resample(WaveFormat fmtin, byte[] buf, int len, WaveFormat fmtout)
        {
            if (fmtin.Channels > 2 || fmtin.Channels < 1)
                throw new NotSupportedException($"Channel '{fmtin.Channels}' is not supported. (Mono / Stereo only)");

            var blockAlign = fmtin.BlockAlign;
            var N = len / blockAlign;
            var sumRate = 0;

            for (var i = 0; i < N; i++)
            {
                float L, R;
                switch (fmtin.BitsPerSample)
                {
                    case 8:
                        L = (buf[i * blockAlign] - 128) / 128f;
                        R = fmtin.Channels == 1 ? L : (buf[i * blockAlign + blockAlign / 2] - 128) / 128f;
                        break;
                    case 16:
                        L = BitConverter.ToInt16(buf, i * blockAlign) / 32768f;
                        R = fmtin.Channels == 1 ? L : BitConverter.ToInt16(buf, i * blockAlign + blockAlign / 2) / 32768f;
                        break;
                    case 32:
                        L = BitConverter.ToSingle(buf, i * blockAlign);
                        R = fmtin.Channels == 1 ? L : BitConverter.ToSingle(buf, i * blockAlign + blockAlign / 2);
                        break;
                    default:
                        throw new NotSupportedException($"BitsPerSample '{fmtin.BitsPerSample}' is not supported. (8 / 16 / 32 only)");
                }

                // OUTPUT
                sumRate += fmtout.SampleRate;
                while (sumRate >= fmtin.SampleRate)
                {
                    sumRate -= fmtin.SampleRate;

                    switch (fmtout.Channels)
                    {
                        case 1:
                            yield return (L + R) / 2;
                            break;
                        case 2:
                            yield return L;
                            yield return R;
                            break;
                        default:
                            throw new NotSupportedException($"Channel '{fmtout.Channels}' is not supported. (Mono / Stereo only)");
                    }
                }
            }
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
