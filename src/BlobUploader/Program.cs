using System;
using BlobUploader.Helpers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace BlobUploader
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
              .ConfigureAppConfiguration((hostingContext, config) =>
              {
                  config.AddJsonFile("appsettings.Development.json", optional: true);
                  config.AddEnvironmentVariables();

                  if (args != null)
                  {
                      config.AddCommandLine(args);
                  }
              })
              .ConfigureServices((hostContext, services) =>
              {
                  services.AddOptions();
                  services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));

                  services.AddSingleton<IHostedService, BlobService>();
              })
              .ConfigureLogging((hostingContext, logging) =>
              {
                  logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                  logging.AddConsole();
              });

            await builder.RunConsoleAsync();
        }
    }
}