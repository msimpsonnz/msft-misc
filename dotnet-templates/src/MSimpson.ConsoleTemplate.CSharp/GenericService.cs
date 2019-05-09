using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MSimpson.ConsoleTemplate
{
    public class GenericService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<AppConfig> _appConfig;

        public GenericService(ILogger<GenericService> logger,  IOptions<AppConfig> appConfig)
        {
            _logger = logger;
            _appConfig = appConfig;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Blob Service is starting.");

            try
            {
                
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to execute application: {ex.Message}");
            }

            return;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping");

            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}
