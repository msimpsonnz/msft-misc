using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NoSQL.ConsoleApp
{
    class Program
    {

        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
              .ConfigureAppConfiguration((hostingContext, config) =>
              {
                  config.AddJsonFile("appsettings.json", optional: true);
                  config.AddJsonFile($"appsettings.Development.json", optional: true);
              })
              .ConfigureServices((hostContext, services) =>
              {
                  services.AddOptions();
                  services.Configure<CosmosConfig>(hostContext.Configuration.GetSection("Cosmos"));
                  services.AddSingleton<IHostedService, CosmosService>();

                  services.Configure<DynamoConfig>(hostContext.Configuration.GetSection("Dynamo"));
                  services.AddSingleton<IHostedService, DynamoService>();

              })
              .ConfigureLogging((hostingContext, logging) => {
                  logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                  logging.AddConsole();
              });

            await builder.RunConsoleAsync();

        }
       
    }
}