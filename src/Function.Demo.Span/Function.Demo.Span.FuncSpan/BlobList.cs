using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Function.Demo.Span.Common;

namespace Function.Demo.Span.FuncSpan
{
    public static class BlobList
    {
        [FunctionName("EventGridSpan")]
        public static void EventGridSpan([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            var blobName = FileHelper.GetBlobNameWithSpan(eventGridEvent);
            log.LogInformation(blobName.ToString());
        }
    }
}