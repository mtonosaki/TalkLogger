// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureAudioMic2 : CoreFeatureBase, ITokenListener
    {
        public NamedId TokenTriggerID => TokenSettingsLoaded;
        private SpeechHandler Handler;

        public override void OnInitInstance()
        {
            base.OnInitInstance();

            Hot.AddRowID(ID.Value, 211, 42);   // Device 2 : Mic
            Hot.AddRowID(0x8000 | ID.Value, 212, 4);    // Blank Space
            Pane.Control.FindForm().FormClosing += FeatureAudioLoopback_FormClosing;

        }
        public override void Start(NamedId who)
        {
            base.Start(who);

            Handler = new SpeechHandler();
            _ = Setup(Handler);
        }

        private async Task Setup(SpeechHandler handler)
        {
            var taskList = new Func<SpeechHandler, Task<bool>>[] {
                            Reset,
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
            _ = Reset(null);
        }

        private class SpeechHandler
        {
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

        private async Task<bool> Reset(SpeechHandler _)
        {
            Handler?.FireStop();
            if (Handler?.Recognizer != null)
            {
                await Handler.Recognizer.StopContinuousRecognitionAsync();
            }
            Handler = null;
            return true;
        }
        private async Task<bool> SelectAudioDeviceAsync(SpeechHandler handler)
        {
            // SELECT A AUDIO DEVICE
            var devices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            foreach (var device in devices)
            {
                if (device.ID == Hot.Setting.Device2ID)
                {
                    handler.Device = device;
                    LOG.WriteMesLine("FeatureAudioMic2", "DeviceInitialized", device.FriendlyName);
                    return true;
                }
            }
            LOG.WriteMesLine("FeatureAudioMic2", "NoDeviceID", Hot.Setting.Device2ID);
            await Task.Delay(20);
            return false;
        }

        private async Task<bool> MakeAudioConfigAsync(SpeechHandler handler)
        {
            Debug.Assert(handler.Device != null);

            var wavein = new WasapiCapture(handler.Device) {
                ShareMode = AudioClientShareMode.Shared
            };
            var waveoutFormat = new WaveFormat(16000, 16, 1);
            var lastSpeakDT = DateTime.Now;
            var willStop = DateTime.MaxValue;
            wavein.StartRecording();

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
                            handler.AudioInputStream.Write(handler.buf, len);
                        }
                        lastSpeakDT = DateTime.Now;
                    }
                }
            };

            var audioformat = AudioStreamFormat.GetWaveFormatPCM(samplesPerSecond: 16000, bitsPerSample: 16, channels: 1);
            handler.AudioInputStream = AudioInputStream.CreatePushStream(audioformat);
            handler.AudioConfig = AudioConfig.FromStreamInput(handler.AudioInputStream);

            await Task.Delay(100);
            handler.StopRequested += (s, e) => {
                wavein.StopRecording();
            };

            return true;
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

        private string TalkID = null;

        private void OnSpeechStartDetected(object sender, RecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"LB.Start : {e}");
        }
        private void OnSpeechEndDetected(object sender, RecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"LB.End : {e}");
        }

        private void OnSessionStarted(object sender, SessionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"LB.OnSessionStarted : {e}");
        }

        private void OnSessionStopped(object sender, SessionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"LB.OnSessionStopped : {e}");
        }

        private void OnRecognizing(object sender, SpeechRecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"LB.R'ng : {e.Result.Text}");

            if (TalkID == null)
            {
                TalkID = Guid.NewGuid().ToString();
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                    RowID = ID.Value,
                    Action = SpeechEvent.Actions.Start,
                    TimeGenerated = DateTime.Now,
                    SessionID = TalkID,
                });
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                    RowID = ID.Value,
                    Action = SpeechEvent.Actions.SetColor,
                    TimeGenerated = DateTime.Now,
                    SessionID = TalkID,
                    Text = Color.FromArgb(64, Color.DarkCyan).ToArgb().ToString(),
                });
            }

            Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                RowID = ID.Value,
                Action = SpeechEvent.Actions.Recognizing,
                TimeGenerated = DateTime.Now,
                SessionID = TalkID,
                Text = e.Result.Text,
            });
            Token.Add(TokenSpeechEventQueued, this);
            GetRoot().FlushFeatureTriggers();
        }

        private void OnRecognized(object sender, SpeechRecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"LB.R'ed : {e.Result.Text}");
            Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                RowID = ID.Value,
                Action = SpeechEvent.Actions.Recognized,
                TimeGenerated = DateTime.Now,
                SessionID = TalkID,
                Text = e.Result.Text,
            });
            TalkID = null;
            Token.Add(TokenSpeechEventQueued, this);
            GetRoot().FlushFeatureTriggers();
        }
        private void OnCancel(object sender, SpeechRecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"LB.OnCancel : {e.Result.Text}");
            Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                RowID = ID.Value,
                Action = SpeechEvent.Actions.Canceled,
                TimeGenerated = DateTime.Now,
                SessionID = TalkID,
                Text = e.Result.Text,
            });
            TalkID = null;
            Token.Add(TokenSpeechEventQueued, this);
            GetRoot().FlushFeatureTriggers();
        }
    }
}
