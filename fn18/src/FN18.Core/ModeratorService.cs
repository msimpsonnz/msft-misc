using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FN18.Core
{
    public class ModeratorService : IModeratorService
    {
        public static ContentModeratorClient client;
        
        public async Task<ModeratorResult> ScoreText(string text)
        {
            GetContentModeratorClient();
            var stream = CreateStream(text);
            var screen = await TextScreen(stream);

            var result = new ModeratorResult()
            {
                Id = screen.TrackingId,
                OriginalText = screen.OriginalText,
                EmailDetected = "none",
                EmailText = "none",
                Term = "none",
                Flagged = false
            };

            if (screen.PII.Email.Count > 0)
            {
                result.EmailDetected = screen.PII.Email[0].Detected;
                result.EmailText = screen.PII.Email[0].Text;
                result.Flagged = true;
            }

            if (screen.Terms != null)
            {
                result.Term = screen.Terms[0].Term;
                result.Flagged = true;
            }

            return result;
        }

        private static MemoryStream CreateStream(string text)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(text);
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;
        }

        public ContentModeratorClient GetContentModeratorClient()
        {
            client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("ContentModeratorKey")));
            client.Endpoint = Environment.GetEnvironmentVariable("ContentModeratorEndpoint");
            return client;
        }

        public async Task<Screen> TextScreen(MemoryStream stream)
        {
            try
            {
                var screenResult = await client
                    .TextModeration
                    .ScreenTextAsync("text/plain", stream, "eng", true, true, "14", true);

                return screenResult;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}
