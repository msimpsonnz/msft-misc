using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Function.Demo.Span.Common;
using System;

namespace Function.Demo.Span.FuncString
{
    public static class BlobList
    {
        [FunctionName("EventGridSubString")]
        public static void EventGridSubString(
            [EventGridTrigger]EventGridEvent eventGridEvent,
            [CosmosDB(
            databaseName: "db",
            collectionName: "coll",
            ConnectionStringSetting = "CosmosDBConnection")] out Event document,
            ILogger log)
        {
            FileHelper fileHelper = new FileHelper();
            document = fileHelper.CreateEventWithString(eventGridEvent);
        }
    }
}