using Microsoft.Azure.CosmosDB.BulkExecutor;
using Microsoft.Azure.CosmosDB.BulkExecutor.BulkImport;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NoSQL.Infrastructure.Cosmos
{
    public class BulkImport
    {
        public static async Task BulkImportDocuments(List<string> documentsToImportInBatch)
        {
            string EndpointUrl = Environment.GetEnvironmentVariable("EndpointUrl");
            string AuthorizationKey = Environment.GetEnvironmentVariable("AuthorizationKey");
            DocumentClient _client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
            DocumentCollection dataCollection = Utils.GetCollectionIfExists(_client, "db", "coll");
            IBulkExecutor bulkExecutor = new BulkExecutor(_client, dataCollection);
            await bulkExecutor.InitializeAsync();
            BulkImportResponse bulkImportResponse = null;
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            try
            {
                bulkImportResponse = await bulkExecutor.BulkImportAsync(
                    documents: documentsToImportInBatch,
                    enableUpsert: true,
                    disableAutomaticIdGeneration: true,
                    maxConcurrencyPerPartitionKeyRange: null,
                    maxInMemorySortingBatchSize: null,
                    cancellationToken: token);
            }
            catch (DocumentClientException de)
            {
                Console.WriteLine("Document _client exception: {0}", de);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
                throw;
            }
            //return bulkImportResponse;

        }
    }
}
