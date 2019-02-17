using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portal.Functions
{
    public static class OnDocumentChanged
    {
        [FunctionName("OnDocumentChanged")]
        public static async Task Run(
            [CosmosDBTrigger("%Cosmos:DatabaseName%", "%Cosmos:CollectionName%", ConnectionStringSetting = "Cosmos:ConnectionString", CreateLeaseCollectionIfNotExists = true)]
                IEnumerable<object> updatedDocs,
            [SignalR(HubName = "submissions")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            foreach (var doc in updatedDocs)
            {
                await signalRMessages.AddAsync(new SignalRMessage
                {
                    Target = "submissionUpdated",
                    Arguments = new[] { "update" }
                });
            }
        }
    }
}
