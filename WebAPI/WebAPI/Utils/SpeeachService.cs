using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.CognitiveServices.Speech;

namespace WebAPI.Utils
{
    public class SpeeachService
    {



        static string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
        static string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");

        public static void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
        {


            switch (speechSynthesisResult.Reason)
            {
                case ResultReason.SynthesizingAudioCompleted:
                    Console.WriteLine($"Speech synthesized for text: [{text}]");
                    break;
                case ResultReason.Canceled:
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                    }
                    break;
                default:
                    break;
            }

        }

        async static Task Main()
        {
            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);

            // The neural multilingual voice can speak different languages based on the input text.
            speechConfig.SpeechSynthesisVoiceName = "en-US-AvaMultilingualNeural";

            using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
            {
                // Get text from the console and synthesize to the default speaker.
                Console.WriteLine("Enter some text that you want to speak >");
                string text = Console.ReadLine();

                var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
                OutputSpeechSynthesisResult(speechSynthesisResult, text);
            }

        }
    }
}
