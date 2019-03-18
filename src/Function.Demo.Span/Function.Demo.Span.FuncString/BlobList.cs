using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Function.Demo.Span.Common;

namespace Function.Demo.Span.FuncString
{
    public static class BlobList
    {
        [FunctionName("EventGridSubString")]
        public static void EventGridSubString([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            var blobName = FileHelper.GetBlobName(eventGridEvent);
            log.LogInformation(blobName);
        }
    }
}