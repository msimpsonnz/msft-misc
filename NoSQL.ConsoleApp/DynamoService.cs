using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
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
        private readonly IOptions<DynamoConfig> _dynamoConfig;
        private AmazonDynamoDBClient _client;
        private Task _task;

        public DynamoService(ILogger<CosmosService> logger, IOptions<DynamoConfig> dynamoConfig)
        {
            _logger = logger;
            _dynamoConfig = dynamoConfig;
            var credentials = new BasicAWSCredentials(_dynamoConfig.Value.AccessKey, _dynamoConfig.Value.SecretKey);
            _client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);

        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting");
            //_task = Task.Factory.StartNew(() => CosmosHelper.RunBulkImportAsync(_client, _cosmosConfig));
            //_task = Task.Factory.StartNew(() => CosmosHelper.GetPartitionKeys(_client, _cosmosConfig));

            //_task = Task.Factory.StartNew(() => CosmosHelper.RunQueryByProp(_client, _cosmosConfig, "47d1fe45-667f-4a8d-9e16-a2caba598172", true));
            //_task = Task.Factory.StartNew(() => CosmosHelper.RunQueryById(_client, _cosmosConfig, "47d1fe45-667f-4a8d-9e16-a2caba598172", true));

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
