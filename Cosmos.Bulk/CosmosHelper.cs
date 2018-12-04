using Microsoft.Azure.CosmosDB.BulkExecutor;
using Microsoft.Azure.CosmosDB.BulkExecutor.BulkImport;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cosmos.Bulk
{
    public class CosmosHelper
    {




        public static async Task RunBulkImportAsync(DocumentClient _client, IOptions<CosmosConfig> _cosmosConfig)
        {
            // Cleanup on start if set in config.

            DocumentCollection dataCollection = await SetupCosmosCollection(_client, _cosmosConfig);

            // Prepare for bulk import.

            // Creating documents with simple partition key here.
            string partitionKeyProperty = dataCollection.PartitionKey.Paths[0].Replace("/", "");

            int numberOfDocumentsToGenerate = _cosmosConfig.Value.NumberOfDocumentsToImport;
            int numberOfBatches = _cosmosConfig.Value.NumberOfBatches;
            long numberOfDocumentsPerBatch = (long)Math.Floor(((double)numberOfDocumentsToGenerate) / numberOfBatches);

            // Set retry options high for initialization (default values).
            _client.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 30;
            _client.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 9;

            IBulkExecutor bulkExecutor = new BulkExecutor(_client, dataCollection);
            await bulkExecutor.InitializeAsync();

            // Set retries to 0 to pass control to bulk executor.
            _client.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 0;
            _client.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 0;

            BulkImportResponse bulkImportResponse = null;
            long totalNumberOfDocumentsInserted = 0;
            double totalRequestUnitsConsumed = 0;
            double totalTimeTakenSec = 0;

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            for (int i = 0; i < numberOfBatches; i++)
            {
                // Generate JSON-serialized documents to import.

                List<string> documentsToImportInBatch = new List<string>();
                long prefix = i * numberOfDocumentsPerBatch;

                Console.Write(String.Format("Generating {0} documents to import for batch {1}", numberOfDocumentsPerBatch, i));
                for (int j = 0; j < numberOfDocumentsPerBatch; j++)
                {
                    string partitionKeyValue = GetPartitionKey(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
                    string id = partitionKeyValue + Guid.NewGuid().ToString();

                    documentsToImportInBatch.Add(Utils.GenerateRandomDocumentString(id, partitionKeyProperty, partitionKeyValue));
                }

                // Invoke bulk import API.

                var tasks = new List<Task>();

                tasks.Add(Task.Run(async () =>
                {
                    Console.WriteLine(String.Format("Executing bulk import for batch {0}", i));
                    do
                    {
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
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception: {0}", e);
                            break;
                        }
                    } while (bulkImportResponse.NumberOfDocumentsImported < documentsToImportInBatch.Count);

                    Console.WriteLine(String.Format("\nSummary for batch {0}:", i));
                    Console.WriteLine("--------------------------------------------------------------------- ");
                    Console.WriteLine(String.Format("Inserted {0} docs @ {1} writes/s, {2} RU/s in {3} sec",
                        bulkImportResponse.NumberOfDocumentsImported,
                        Math.Round(bulkImportResponse.NumberOfDocumentsImported / bulkImportResponse.TotalTimeTaken.TotalSeconds),
                        Math.Round(bulkImportResponse.TotalRequestUnitsConsumed / bulkImportResponse.TotalTimeTaken.TotalSeconds),
                        bulkImportResponse.TotalTimeTaken.TotalSeconds));
                    Console.WriteLine(String.Format("Average RU consumption per document: {0}",
                        (bulkImportResponse.TotalRequestUnitsConsumed / bulkImportResponse.NumberOfDocumentsImported)));
                    Console.WriteLine("---------------------------------------------------------------------\n ");

                    totalNumberOfDocumentsInserted += bulkImportResponse.NumberOfDocumentsImported;
                    totalRequestUnitsConsumed += bulkImportResponse.TotalRequestUnitsConsumed;
                    totalTimeTakenSec += bulkImportResponse.TotalTimeTaken.TotalSeconds;
                },
                token));

                /*
                tasks.Add(Task.Run(() =>
                {
                    char ch = Console.ReadKey(true).KeyChar;
                    if (ch == 'c' || ch == 'C')
                    {
                        tokenSource.Cancel();
                        Console.WriteLine("\nTask cancellation requested.");
                    }
                }));
                */

                await Task.WhenAll(tasks);
            }

            Console.WriteLine("Overall summary:");
            Console.WriteLine("--------------------------------------------------------------------- ");
            Console.WriteLine(String.Format("Inserted {0} docs @ {1} writes/s, {2} RU/s in {3} sec",
                totalNumberOfDocumentsInserted,
                Math.Round(totalNumberOfDocumentsInserted / totalTimeTakenSec),
                Math.Round(totalRequestUnitsConsumed / totalTimeTakenSec),
                totalTimeTakenSec));
            Console.WriteLine(String.Format("Average RU consumption per document: {0}",
                (totalRequestUnitsConsumed / totalNumberOfDocumentsInserted)));
            Console.WriteLine("--------------------------------------------------------------------- ");

            // Cleanup on finish if set in config.

            if (_cosmosConfig.Value.ShouldCleanupOnFinish)
            {
                Console.WriteLine("Deleting Database {0}", _cosmosConfig.Value.DatabaseName);
                await _client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(_cosmosConfig.Value.DatabaseName));
            }

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }

        private static string GetPartitionKey(int partitionKey)
        {
            SHA256 sha256 = SHA256.Create();
            int bucket = partitionKey % 256;
            var partition = sha256.ComputeHash(BitConverter.GetBytes(bucket));
            var hash = GetStringFromHash(partition);
            return hash;

        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        private static async Task<DocumentCollection> SetupCosmosCollection(DocumentClient _client, IOptions<CosmosConfig> _cosmosConfig)
        {
            DocumentCollection dataCollection = null;
            try
            {
                if (_cosmosConfig.Value.ShouldCleanupOnStart)
                {
                    Database database = Utils.GetDatabaseIfExists(_client, _cosmosConfig.Value.DatabaseName);
                    if (database != null)
                    {
                        await _client.DeleteDatabaseAsync(database.SelfLink);
                    }

                    Console.WriteLine("Creating database {0}", _cosmosConfig.Value.DatabaseName);
                    database = await _client.CreateDatabaseAsync(new Database { Id = _cosmosConfig.Value.DatabaseName });

                    Console.WriteLine(String.Format("Creating collection {0} with {1} RU/s", _cosmosConfig.Value.CollectionName, _cosmosConfig.Value.CollectionThroughput));
                    dataCollection = await Utils.CreatePartitionedCollectionAsync(_client, _cosmosConfig.Value.DatabaseName, _cosmosConfig.Value.CollectionPartitionKey, _cosmosConfig.Value.CollectionName, _cosmosConfig.Value.CollectionThroughput);
                }
                else
                {
                    dataCollection = Utils.GetCollectionIfExists(_client, _cosmosConfig.Value.DatabaseName, _cosmosConfig.Value.CollectionName);
                    if (dataCollection == null)
                    {
                        throw new Exception("The data collection does not exist");
                    }
                }
            }
            catch (Exception de)
            {
                Console.WriteLine("Unable to initialize, exception message: {0}", de.Message);
                throw;
            }

            return dataCollection;
        }

    }
}