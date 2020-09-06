using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using PocSpeechService;

namespace Speech.Recognition
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Talk to mic...");
            await RecognizeSpeechAsync();
        }

        static async Task RecognizeSpeechAsync()
        {
            var param = new MySecretParameter();
            var config = SpeechConfig.FromSubscription(param.SubscriptionKey, param.ServiceRegion);

            using var recognizer = new SpeechRecognizer(config);

            var result = await recognizer.RecognizeOnceAsync();
            switch (result.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    Console.WriteLine($"We recognized: {result.Text}");
                    break;
                case ResultReason.NoMatch:
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                    break;
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                    break;
            }
        }
    }
}