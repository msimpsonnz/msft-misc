using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NoSQL.ConsoleApp
{
    public class DynamoService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<CosmosConfig> _cosmosConfig;
        private DocumentClient _client;
        private Task _task;

        public DynamoService(ILogger<CosmosService> logger, IOptions<CosmosConfig> cosmosConfig)
        {
            _logger = logger;
            _cosmosConfig = cosmosConfig;
            _client = new DocumentClient(
                    new Uri(_cosmosConfig.Value.EndpointUrl),
                    _cosmosConfig.Value.AuthorizationKey,
                    );

        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting");
            //_task = Task.Factory.StartNew(() => CosmosHelper.RunBulkImportAsync(_client, _cosmosConfig));
            //_task = Task.Factory.StartNew(() => CosmosHelper.GetPartitionKeys(_client, _cosmosConfig));

            _task = Task.Factory.StartNew(() => CosmosHelper.RunQueryByProp(_client, _cosmosConfig, "47d1fe45-667f-4a8d-9e16-a2caba598172", true));
            _task = Task.Factory.StartNew(() => CosmosHelper.RunQueryById(_client, _cosmosConfig, "47d1fe45-667f-4a8d-9e16-a2caba598172", true));

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
    }
}
