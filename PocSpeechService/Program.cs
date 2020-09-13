using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using PocSpeechService;

namespace Speech.Recognition
{
    class Program
    {
        static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            await RecognizeSpeechAsync();
            Console.WriteLine("Talk to mic...Push Enter key to exit.");

            Console.In.ReadLine();
            Console.WriteLine($"Stopping....");
            await recognizer.StopContinuousRecognitionAsync();

            Console.WriteLine($"END OF PROGRAM");
        }

        private static SpeechRecognizer recognizer;

        private static async Task RecognizeSpeechAsync()
        {
            var param = new MySecretParameter();
            //var audioformat = AudioStreamFormat.GetWaveFormatPCM(samplesPerSecond: 16000, bitsPerSample: 16, channels: 1);
            //var audioStream = AudioInputStream.CreatePushStream(audioformat);
            //var audioConfig = AudioConfig.FromStreamInput(audioStream);

            //var audioConfig = AudioConfig.FromWavFileInput(@"D:\Users\ManabuTonosaki\Documents\tono.wav");

            var audioConfig = AudioConfig.FromDefaultMicrophoneInput();

            recognizer = new SpeechRecognizer(SpeechConfig.FromSubscription(param.SubscriptionKey, param.ServiceRegion), "ja-JP", audioConfig);
            recognizer.Recognizing += OnRecognizing;
            recognizer.Recognized += OnRecognized;
            recognizer.Canceled += OnCancel;
            recognizer.SessionStarted += OnSessionStarted;
            recognizer.SessionStopped += OnSessionStopped;
            recognizer.SpeechStartDetected += OnSpeechStartDetected;
            recognizer.SpeechEndDetected += OnSpeechEndDetected;

            Console.WriteLine($"Starting....");
            await recognizer.StartContinuousRecognitionAsync();
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