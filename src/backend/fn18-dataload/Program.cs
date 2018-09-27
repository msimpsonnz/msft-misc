using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;

namespace fn18_dataload
{
    class Program
    {
        public static IConfiguration config { get; set; }
        private DocumentClient client;
        Dictionary<int, string> posUrls = new Dictionary<int, string>();
        Dictionary<int, string> negUrls = new Dictionary<int, string>();

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json");
            config = builder.Build();

            Program p = new Program();
            p.GenerateComments(config).Wait();
        }

        public async Task GenerateComments(IConfiguration config)
        {
            GetPosUrls();
            GetNegUrls();
            List<Comment> comments = new List<Comment>();
            var targetDirectory = @"C:\temp\comments";
            var files = Directory.GetFiles(targetDirectory, "*.*", SearchOption.AllDirectories);
            foreach (var comment in files)
            {
                comments.Add(GetFile(comment));
            }

            System.Console.WriteLine();
            // foreach(var Comment in comments)
            // {
            //     await CreateDoc(config, Comment);
            //     System.Console.WriteLine(Comment.CommentId);
            // }
            string result = JsonConvert.SerializeObject(comments);
            File.WriteAllText(@"C:\temp\comments.json", result);

        }


        public void GetPosUrls()
        {
            int counter = 0;
            string line;
            StreamReader file = new System.IO.StreamReader(@"C:\temp\urls\urls_pos.txt");
            while ((line = file.ReadLine()) != null)
            {
                posUrls.Add(counter, line);
                counter++;
            }
        }

        public void GetNegUrls()
        {
            int counter = 0;
            string line;
            StreamReader file = new System.IO.StreamReader(@"C:\temp\urls\urls_neg.txt");
            while ((line = file.ReadLine()) != null)
            {
                negUrls.Add(counter, line);
                counter++;
            }
        }


        public Comment GetFile(string file)
        {
            var dir = file.Split('\\');
            if (dir[2] == "pos")
            {
            string fileName = Path.GetFileNameWithoutExtension(file);
            var idText = fileName.Split('_');
            int id;
            int.TryParse(idText[0], out id);
            string text = File.ReadAllText(file);
            Comment comment = new Comment
            {
                Type = "comment",
                Sentiment = "pos",
                CommentId = id,
                CommentUrl = posUrls[id].Replace("/usercomments", ""),
                Text = text
            };
            return comment;
            }
            else
            {
            string fileName = Path.GetFileNameWithoutExtension(file);
            var idText = fileName.Split('_');
            int id;
            int.TryParse(idText[0], out id);
            string text = File.ReadAllText(file);
            Comment comment = new Comment
            {
                Type = "comment",
                Sentiment = "neg",
                CommentId = id,
                CommentUrl = negUrls[id].Replace("/usercomments", ""),
                Text = text
            };
            return comment;
            }

        }

        public async Task CreateDoc(IConfiguration config, object doc)
        {

            this.client = new DocumentClient(new Uri(config["EndpointUri"]), config["PrimaryKey"]);

            await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(config["Database"], config["Collection"]), doc);
        }
    }

    public class Comment
    {
        public string Type { get; set; }
        public string Sentiment { get; set; }
        public int CommentId { get; set; }
        public string CommentUrl { get; set; }
        public string Text { get; set; }
    }
}
