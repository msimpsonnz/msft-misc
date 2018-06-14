using Microsoft.Extensions.Configuration;
using Microsoft.Bot.Connector.DirectLine;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

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
            DirectLineClient client = new DirectLineClient(directLineSecret);

            var conversation = await client.Conversations.StartConversationAsync();

            new System.Threading.Thread(async () => await ReadBotMessagesAsync(client, conversation.ConversationId, botId)).Start();

            Console.WriteLine(artlistList);
            Activity userMessage = new Activity
            {
                From = new ChannelAccount("DirectLineSampleClientUser"),
                Text = $"i like {artlistList}",
                Type = ActivityTypes.Message
            };

            var msg = await client.Conversations.PostActivityAsync(conversation.ConversationId, userMessage);
            msg.Id.ToString();

        }

        private static async Task ReadBotMessagesAsync(DirectLineClient client, string conversationId, string botId)
        {
            string watermark = null;

            while (true)
            {
                var activitySet = await client.Conversations.GetActivitiesAsync(conversationId, watermark);
                watermark = activitySet?.Watermark;

                var activities = from x in activitySet.Activities
                                 where x.From.Id == botId
                                 select x;

                var str = "";

                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            }
        }
    }


    public class Message
    {
        public string type { get; set; }
        public Message from { get; set; }
        public string text { get; set; }
    }

    public class MessageFrom
    {
        public string id { get; set; }
    }
}
