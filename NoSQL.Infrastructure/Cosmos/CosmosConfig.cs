using Microsoft.Azure.Documents.Client;

namespace NoSQL.Infrastructure
{
    public class CosmosConfig
    {

        public string EndpointUrl { get; set; }
        public string AuthorizationKey { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public int CollectionThroughput { get; set; }
        public string CollectionPartitionKey { get; set; }
        public bool ShouldCleanupOnStart { get; set; }
        public bool ShouldCleanupOnFinish { get; set; }
        public int NumberOfDocumentsToImport { get; set; }
        public int NumberOfBatches { get; set; }

    }

}
