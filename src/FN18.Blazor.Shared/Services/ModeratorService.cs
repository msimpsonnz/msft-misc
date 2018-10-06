using FN18.Blazor.Shared.Interfaces;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FN18.Blazor.Shared.Services
{
    public class ModeratorService : IModeratorService
    {
        public IConfiguration _configuration { get; set; }
        public static ContentModeratorClient client;
        public ModeratorService(IConfiguration configuration) => _configuration = configuration;


        public async Task<Screen> GetModeratorClient(string text)
        {
            GetContentModeratorClient(_configuration);
            MemoryStream stream = CreateStream(text);
            var result = await TextScreen(stream);
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
