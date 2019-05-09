using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using NoSQL.Infrastructure;
using NoSQL.Infrastructure.Cosmos;

namespace NoSQL.FunctionLoader
{
    public static class BulkLoader
    {
        [FunctionName("BulkLoader")]
        public static async Task<bool> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            int batchSize;
            int.TryParse(Environment.GetEnvironmentVariable("BatchSize"), out batchSize);

            int firstRetryIntervalVar;
            int.TryParse(Environment.GetEnvironmentVariable("FirstRetrySeconds"), out firstRetryIntervalVar);

            int maxNumberOfAttemptsVar;
            int.TryParse(Environment.GetEnvironmentVariable("MaxNumberOfAttempts"), out maxNumberOfAttemptsVar);

            double backoffCoefficientVar;
            double.TryParse(Environment.GetEnvironmentVariable("BackoffCoefficient"), out backoffCoefficientVar);

            var retryOptions = new RetryOptions(
                firstRetryInterval: TimeSpan.FromSeconds(firstRetryIntervalVar),
                maxNumberOfAttempts: maxNumberOfAttemptsVar);
            retryOptions.BackoffCoefficient = backoffCoefficientVar;

            var outputs = new List<bool>();
            var tasks = new Task<bool>[batchSize];
            for (int i = 0; i < batchSize; i++)
            {
                tasks[i] = context.CallActivityWithRetryAsync<bool>("BulkLoader_Batch", retryOptions, i);
            }

            await Task.WhenAll(tasks);
            return true;
        }

        [FunctionName("BulkLoader_Batch")]
        public static async Task<bool> Import([ActivityTrigger] int batch, ILogger log)
        {
            log.LogInformation($"Prepare documents for batch {batch}");
            string partitionKey = Environment.GetEnvironmentVariable("PartitionKey");
            int docsPerBatch;
            int.TryParse(Environment.GetEnvironmentVariable("DocsPerBatch"), out docsPerBatch);
            List<string> documentsToImportInBatch = CosmosHelper.DocumentBatch(partitionKey, docsPerBatch, batch);
            await BulkImport.BulkImportDocuments(documentsToImportInBatch);
            return true;
        }

        [FunctionName("BulkLoader_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("BulkLoader", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }



    }
}