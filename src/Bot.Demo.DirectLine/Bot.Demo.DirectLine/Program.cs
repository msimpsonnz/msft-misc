using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Bot.Demo.DirectLine
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json");
            var configuration = builder.Build();

            string directLineSecret = configuration["RingoDirectLine"];
            string botId = configuration["RingoBotId"];
            string artlistList = "metallica";

            LikeArtist(directLineSecret, botId, artlistList).Wait();
        }


        public static async Task LikeArtist(string directLineSecret, string botId, string artlistList)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", directLineSecret);

            var convIdRequest = await client.PostAsync("https://directline.botframework.com/v3/directline/conversations", null);

            var convIdResult = await convIdRequest.Content.ReadAsStringAsync();
            JObject convIdObj = JObject.Parse(convIdResult);
            string convId = (string)convIdObj["conversationId"];


            Message msg = new Message()
            {
                type = "message",
                from = new MessageFrom() { id = "RingoDirectLineSampleUser" },
                text = "i like spotify:artist:7n2wHs1TKAczGzO7Dd2rGr"

            };
            var msgJson = JsonConvert.SerializeObject(msg);

            var sendArtisit = await client.PostAsync($"https://directline.botframework.com/v3/directline/conversations/{convId}/activities", new StringContent(msgJson, Encoding.UTF8, "application/json"));

            var sendArtisitResult = await sendArtisit.Content.ReadAsStringAsync();
        }


    }

    public class Message
    {
        public string type { get; set; }
        public MessageFrom from { get; set; }
        public string text { get; set; }
    }

    public class MessageFrom
    {
        public string id { get; set; }
    }

}
