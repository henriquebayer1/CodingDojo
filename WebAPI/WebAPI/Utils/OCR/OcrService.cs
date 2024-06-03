using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace WebAPI.Utils.OCR
{
    public class OcrService
    {

        private readonly string _subscriptionKey = "e255f8e9cf4c40e68f86618f480301f1";

        private readonly string _endPoint = "https://cvvitalhubg06.cognitiveservices.azure.com/";

        public async Task<string> RecognizeTextAsync(Stream imageStream)
        {



            try
            {

                var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_subscriptionKey))
                {

                    Endpoint = _endPoint
                };

                var OcrResult = await client.RecognizePrintedTextInStreamAsync(true, imageStream);

                return ProcessRecognitionResult(OcrResult);



            }
            catch (Exception ex)
            {

                return "Erro ao reconhecer o texto" + ex.Message;
            }
        
        }

        private static string  ProcessRecognitionResult(OcrResult result)
        {
            string recognizedText = "";

            foreach (var region in result.Regions)
            {
                foreach (var line in region.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        recognizedText += word.Text + " ";
                        
                    }

                    //Quebra de linha ao final de cada linha
                    recognizedText += "\n";
                }


            }

            return recognizedText;


        }
        
    }
}
