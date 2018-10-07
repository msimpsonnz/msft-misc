using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FN18.Shared
{
    public class ModeratorService : IModeratorService
    {
        public IConfiguration _configuration { get; set; }
        public static ContentModeratorClient client;
        public ModeratorService(IConfiguration configuration) => _configuration = configuration;


        public async Task<ModeratorResult> GetModeratorClient(string text)
        {
            //GetContentModeratorClient(_configuration);
            //MemoryStream stream = CreateStream(text);
            //var screen = await TextScreen(stream);
            var result = new ModeratorResult()
            {
                //Id = screen.TrackingId,
                //OriginalText = screen.OriginalText,
                ////Email = rawResult.PII.Email[0].ToString() ?? null,
                //Email = string.Empty,
                //Term = screen.Terms[0].Term ?? null
                Id = Guid.NewGuid().ToString(),
                OriginalText = "Original Text",
                Email = "some email",
                Term = "some term",
                Flagged = true

            };
            return result;
        }

        private static MemoryStream CreateStream(string text)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(text);
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;
        }

        public ContentModeratorClient GetContentModeratorClient(IConfiguration _configuration)
        {
            client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(_configuration["ContentModeratorKey"]));
            client.Endpoint = _configuration["ContentModeratorEndpoint"];
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
