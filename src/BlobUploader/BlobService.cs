using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BlobUploader.Helpers;
using Microsoft.Extensions.Options;

namespace BlobUploader
{
    public class BlobService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<AppConfig> _appConfig;

        public BlobService(ILogger<BlobService> logger,  IOptions<AppConfig> appConfig)
        {
            _logger = logger;
            _appConfig = appConfig;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Blob Service is starting.");

            try
            {
                Console.WriteLine("Starting Upload");
                BlobHelper.CreateStorageConnection(_appConfig.Value.ConnectionString, _appConfig.Value.StorageContainer);
                await BlobHelper.UploadSummary(_appConfig.Value.LocalDir, Guid.NewGuid().ToString(), _logger);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to execute application: {ex.Message}");
            }

            return;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}
