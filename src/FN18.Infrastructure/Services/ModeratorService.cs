using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator.Models;
using FN18.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FN18.Infrastructure.Services
{
    public class ModeratorService : IModeratorService
    {
        public IConfiguration _configuration { get; set; }
        public static ContentModeratorClient contentModeratorClient;
        public ModeratorService(IConfiguration configuration) => _configuration = configuration;


        public async Task GetModeratorClient(string text)
        {
            GetContentModeratorClient(_configuration);
            var result = await TextScreen(text);
        }

        public ContentModeratorClient GetContentModeratorClient(IConfiguration _configuration)
        {
            contentModeratorClient = new ContentModeratorClient(new ApiKeyServiceClientCredentials(_configuration["ContentModeratorKey"]));
            contentModeratorClient.Endpoint = _configuration["ContentModeratorEndpoint"];
            return contentModeratorClient;
        }

        public async Task<Screen> TextScreen(string text)
        {
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                MemoryStream stream = new MemoryStream(byteArray);
                var screenResult = await contentModeratorClient.TextModeration.ScreenTextAsync("text/plain", stream, "eng", true, true, null, true);
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
