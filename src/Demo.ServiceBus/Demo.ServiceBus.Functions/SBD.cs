using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Demo.ServiceBus.Functions
{
    public static class SBD
    {
        [FunctionName("SBD")]
        public static void Run([ServiceBusTrigger("graph", "addRelationship", Connection = "AzureWebJobsServiceBus")]string mySbMsg, TraceWriter log)
        {
            log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
