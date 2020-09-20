using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureAudioLoopback : CoreFeatureBase, ITokenListener
    {
        public NamedId TokenTriggerID => TokenSettingsLoaded;
        private SpeechHandler Handler;

        public override void Start(NamedId who)
        {
            base.Start(who);

            Pane.Control.FindForm().FormClosing += FeatureAudioLoopback_FormClosing;
            Handler = new SpeechHandler
            {
                Settings = Hot.Setting,
            };
            _ = Setup(Handler);
        }

        private async void FeatureAudioLoopback_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if(Handler != null)
            {
                Handler.FireStop();
                if(Handler.Recognizer != null)
                {
                    await Handler.Recognizer.StopContinuousRecognitionAsync();
                }
            }
        }

        private async Task Setup(SpeechHandler handler)
        {
            var taskList = new Func<SpeechHandler, Task<bool>>[] {
                            SelectAudioDeviceAsync,
                            MakeAudioConfigAsync,
                            StartRecognizeSpeechAsync,
            };
            foreach (var func in taskList )
            {
                var ret = await func.Invoke(handler);
                if (!ret)
                {
                    break;
                }
            }
        }

        public class SpeechHandler
        {
            public SettingModel Settings { get; set; }
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

        private static async Task<bool> SelectAudioDeviceAsync(SpeechHandler handler)
        {
            // SELECT A AUDIO DEVICE
            var devices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            foreach( var device in devices )
            {
                if (device.ID == handler.Settings.Device1ID)
                {
                    handler.Device = device;
                    return true;
                }
            }
            LOG.WriteMesLine("FeatureAudioLoopback", "NoDeviceID", handler.Settings.Device1ID);
            return false;
        }

        private static async Task<bool> MakeAudioConfigAsync(SpeechHandler handler)
        {
            Debug.Assert(handler.Device != null);

            var wavein = new WasapiLoopbackCapture(handler.Device);
            var waveoutFormat = new WaveFormat(16000, 16, 1);
            var lastSpeakDT = DateTime.Now;
            var willStop = DateTime.MaxValue;

            wavein.DataAvailable += (s, e) =>
            {
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
            handler.StopRequested += (s, e) =>
            {
                wavein.StopRecording();
            };
            wavein.StartRecording();

            return true;
        }
        private static async Task<bool> StartRecognizeSpeechAsync(SpeechHandler handler)
        {
            handler.Recognizer = new SpeechRecognizer(SpeechConfig.FromSubscription(handler.Settings.SubscriptionKey, handler.Settings.ServiceRegion), "ja-JP", handler.AudioConfig);
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
        private static void OnSpeechStartDetected(object sender, RecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"Loopback.OnSpeechStartDetected : {e}");
        }
        private static void OnSpeechEndDetected(object sender, RecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"Loopback.OnSpeechStartDetected : {e}");
        }

        private static void OnSessionStarted(object sender, SessionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"Loopback.OnSessionStarted : {e}");
        }

        private static void OnSessionStopped(object sender, SessionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"Loopback.OnSessionStopped : {e}");
        }

        private static void OnRecognizing(object sender, SpeechRecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"Loopback.OnRecognizing : {e.Result.Text}");
        }

        private static void OnRecognized(object sender, SpeechRecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"Loopback.OnRecognized : {e.Result.Text}");
        }
        private static void OnCancel(object sender, SpeechRecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"Loopback.OnCancel : {e.Result.Text}");
        }
    }
}
