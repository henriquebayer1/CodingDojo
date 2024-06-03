using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using WebAPI.Domains;
using WebAPI.Utils.OCR;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechController : ControllerBase
    {



        [HttpPost]
        public async Task<IActionResult> RecognizeText(string texto)
        {
              string speechKey = "d3a2da1743b5452f86af5de95aab4bad";
        string speechRegion = "eastus";

        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);

            // The neural multilingual voice can speak different languages based on the input text.
            speechConfig.SpeechSynthesisVoiceName = "en-US-AvaMultilingualNeural";

           

            using (AudioConfig audioConfig = AudioConfig.FromWavFileOutput(@"D:\test.wav") )
            {
                using (SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer(speechConfig, audioConfig))
                {
                  
                    string text = texto;

                    SpeechSynthesisResult speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);

                    return Ok(speechSynthesisResult);
                }



            }


        }
    }
}