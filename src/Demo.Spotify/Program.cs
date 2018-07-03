using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using SpotifyApi.NetCore;

namespace Demo.Spotify
{
    class Program
    {
        public static IConfiguration config { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json");

            config = builder.Build();
            Environment.SetEnvironmentVariable("SpotifyApiClientId", config["SpotifyApiClientId"]);
            Environment.SetEnvironmentVariable("SpotifyApiClientSecret", config["SpotifyApiClientSecret"]);

            AsyncMain(args).GetAwaiter().GetResult();
            Console.ReadLine();
        }

        private static async Task AsyncMain(string[] args)
        {
            var http = new HttpClient();
            var auth = new ClientCredentialsAuthorizationApi(http);
            var api = new ArtistsApi(http, auth);

            // Get an artist by Spotify Artist Id
            dynamic response = await api.GetArtist("1tpXaFf2F55E7kVJON4j4G");
            Console.WriteLine(response);

        }
    }
}
