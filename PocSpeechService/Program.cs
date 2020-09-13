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
            var spconfig = SpeechConfig.FromSubscription(param.SubscriptionKey, param.ServiceRegion);
            spconfig.SpeechRecognitionLanguage = "ja-JP";

            recognizer = new SpeechRecognizer(spconfig);
            recognizer.Recognizing += OnRecognizing;
            recognizer.Recognized += OnRecognized;
            recognizer.Canceled += OnCancel;

            Console.WriteLine($"Starting....");
            await recognizer.StartContinuousRecognitionAsync();
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