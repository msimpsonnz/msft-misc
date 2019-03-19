using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Function.Demo.Span.Common;
using System;

namespace Function.Demo.Span.FuncSpan
{
    public static class BlobList
    {
        [FunctionName("EventGridSpan")]
        public static void EventGridSpan(
            [EventGridTrigger]EventGridEvent eventGridEvent,
            [CosmosDB(
            databaseName: "db",
            collectionName: "coll",
            ConnectionStringSetting = "CosmosDBConnection")] out Event document,
            ILogger log)
        {
            FileHelper fileHelper = new FileHelper();
            document = fileHelper.CreateEventWithSpan(eventGridEvent);
        }
    }
}