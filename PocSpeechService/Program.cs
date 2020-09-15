using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using PocSpeechService;

namespace Speech.Recognition
{
    class Program
    {
        static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var handler = new SpeechHandler();
            foreach (var func in
                new Func<SpeechHandler, Task<bool>>[] {
                    SelectAudioDeviceAsync,
                    MakeAudioConfigAsync,
                    StartRecognizeSpeechAsync,
                    ListenAsync,
                    Finalize,
                }
            )
            {
                var ret = await func.Invoke(handler);
                if (!ret)
                {
                    break;
                }
            }
            Console.WriteLine($"END OF PROGRAM");
        }

        public class SpeechHandler
        {
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

        private static async Task<bool> SelectAudioDeviceAsync(SpeechHandler handler)
        {
            // SELECT A AUDIO DEVICE
            var devices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            handler.Device = null;
            while (handler.Device == null)
            {
                Console.WriteLine("Select an audio device.");
                for (var i = 0; i < devices.Count; i++)
                {
                    var device = devices[i];
                    Console.WriteLine($"  [{i + 1}] : {device.FriendlyName}");
                }
                Console.WriteLine("  [Q] Quit.");
                Console.Write("> ");
                try
                {
                    var line = Console.In.ReadLine()?.ToUpper().Trim();
                    if (line == "Q")
                    {
                        return false;
                    }
                    var no = int.Parse(line);
                    if (no > 0 && no <= devices.Count)
                    {
                        handler.Device = devices[no - 1];
                    }
                }
                catch
                {
                }
                await Task.Delay(300);
                Console.WriteLine();
            }
            return true;
        }

        private static async Task<bool> MakeAudioConfigAsync(SpeechHandler handler)
        {
            // var audioConfig = AudioConfig.FromWavFileInput(@"D:\Users\ManabuTonosaki\OneDrive - tomarika\tono.wav");
            // var audioConfig = AudioConfig.FromDefaultMicrophoneInput();

            Debug.Assert(handler.Device != null);

            var wavein = new WasapiLoopbackCapture(handler.Device);
            var waveoutFormat = new WaveFormat(16000, 16, 1);
            var lastSpeakDT = DateTime.Now;
            var buf = new byte[1024 * 48];
            var a = false;

            wavein.DataAvailable += (s, e) =>
            {
                if (e.BytesRecorded > 0)
                {
                    using var ms = new MemoryStream(e.Buffer, 0, e.BytesRecorded);
                    using var rs = new RawSourceWaveStream(ms, wavein.WaveFormat);
                    using var freq = new MediaFoundationResampler(rs, waveoutFormat.SampleRate);
                    var w16 = freq.ToSampleProvider().ToMono().ToWaveProvider16();
                    var len = w16.Read(buf, 0, buf.Length);
                    handler.AudioInputStream.Write(buf, len);
                    lastSpeakDT = DateTime.Now;
                    a = true;
                }
                else
                {
                    var silence = new SilenceProvider(waveoutFormat);
                    var len = silence.Read(buf, 0, waveoutFormat.BitsPerSample * waveoutFormat.SampleRate / 8 / 100);    // 10ms
                    var cnt = (int)((DateTime.Now - lastSpeakDT).TotalMilliseconds / 10);
                    for (var i = 0; i < cnt; i++)
                    {
                        handler.AudioInputStream.Write(buf, len);
                    }
                    lastSpeakDT = DateTime.Now;
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
            var param = new MySecretParameter();
            handler.Recognizer = new SpeechRecognizer(SpeechConfig.FromSubscription(param.SubscriptionKey, param.ServiceRegion), "ja-JP", handler.AudioConfig);
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

        private static async Task<bool> ListenAsync(SpeechHandler handler)
        {
            await Task.Delay(10);
            Console.WriteLine("Talk to mic...Push Enter key to exit.");
            Console.In.ReadLine();
            return true;
        }

        private static async Task<bool> Finalize(SpeechHandler handler)
        {
            Console.WriteLine($"Stopping....");
            handler.FireStop();
            await handler.Recognizer.StopContinuousRecognitionAsync();
            return true;
        }

        private static void OnSpeechStartDetected(object sender, RecognitionEventArgs e)
        {
            Console.WriteLine($"OnSpeechStartDetected : {e}");
        }
        private static void OnSpeechEndDetected(object sender, RecognitionEventArgs e)
        {
            Console.WriteLine($"OnSpeechStartDetected : {e}");
        }

        private static void OnSessionStarted(object sender, SessionEventArgs e)
        {
            Console.WriteLine($"OnSessionStarted : {e}");
        }

        private static void OnSessionStopped(object sender, SessionEventArgs e)
        {
            Console.WriteLine($"OnSessionStopped : {e}");
        }

        private static void OnRecognizing(object sender, SpeechRecognitionEventArgs e)
        {
            Console.WriteLine($"OnRecognizing : {e.Result.Text}");
        }

        private static void OnRecognized(object sender, SpeechRecognitionEventArgs e)
        {
            Console.WriteLine($"OnRecognized : {e.Result.Text}");
        }
        private static void OnCancel(object sender, SpeechRecognitionEventArgs e)
        {
            Console.WriteLine($"OnCancel : {e.Result.Text}");
        }
    }
}