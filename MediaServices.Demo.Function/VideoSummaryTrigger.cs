using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MediaServices.Demo.Function
{
    public static class VideoSummaryTrigger
    {
        [FunctionName("VideoSummaryTrigger")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            dynamic entity = context.GetInput<object>();
            string correlationId = entity["id"];
            string assetId = entity["id"];
            await context.CallActivityAsync("VideoSummary_Runner", (correlationId, assetId));

        }


        [FunctionName("VideoSummary_EventGridStart")]
        public static async Task EventGridStart([EventGridTrigger]EventGridEvent eventGridEvent,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("VideoSummaryTrigger", eventGridEvent);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            //return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}