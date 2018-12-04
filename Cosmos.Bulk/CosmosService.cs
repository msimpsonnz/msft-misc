using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cosmos.Bulk
{
    public class CosmosService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<CosmosConfig> _cosmosConfig;
        private DocumentClient _client;
        private Task _task;

        public CosmosService(ILogger<CosmosService> logger, IOptions<CosmosConfig> cosmosConfig)
        {
            _logger = logger;
            _cosmosConfig = cosmosConfig;
            _client = new DocumentClient(
                    new Uri(_cosmosConfig.Value.EndpointUrl),
                    _cosmosConfig.Value.AuthorizationKey,
                    ConnectionPolicy);

        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting");
            //_task = Task.Factory.StartNew(() => CosmosHelper.RunBulkImportAsync(_client, _cosmosConfig));
            _task = Task.Factory.StartNew(() => CosmosHelper.RunQuery(_client, _cosmosConfig, "FF24BA580CA7F75Ae84d9f9e-9b0f-4f74-924d-931dd31ca6bf", true));
            var partitionKeys = new string[] { "FF24BA580CA7F75A", "5AE7E16F4E28A961", "9AC1C242E646BFC6" };
            _task = Task.Factory.StartNew(() => CosmosHelper.RunQuery(_client, _cosmosConfig, "FF24BA580CA7F75Ae84d9f9e-9b0f-4f74-924d-931dd31ca6bf", partitionKeys));

            return;


        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _task?.Dispose();
        }


        private static readonly ConnectionPolicy ConnectionPolicy = new ConnectionPolicy
        {
            ConnectionMode = ConnectionMode.Direct,
            ConnectionProtocol = Protocol.Tcp
        };
    }
}
