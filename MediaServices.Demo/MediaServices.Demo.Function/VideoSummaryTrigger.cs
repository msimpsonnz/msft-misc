using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MediaServices.Demo.Function.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MediaServices.Demo.Function
{
    public static class VideoSummaryTrigger
    {
        [FunctionName("VideoSummaryTrigger")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context, ILogger log)
        {
            dynamic entity = context.GetInput<object>();
            string correlationId = entity["id"];
            dynamic correlationData = JsonConvert.DeserializeObject<dynamic>(entity["data"]["correlationData"]["metaString"].ToString());
            string assetId = correlationData.outputAssetId; //["outputAssetId"];
            log.LogInformation($"Creating Activity: CorrelationId: {correlationId}, Container: {assetId}");
            await context.CallActivityAsync("VideoSummary_Runner", (correlationId, assetId));
            
        }


        [FunctionName("VideoSummary_EventGridStart")]
        public static async Task EventGridStart([EventGridTrigger]JObject eventGridEvent,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {

            var jobStateChange = eventGridEvent.ToObject<JobStateChange>();
            if (!(jobStateChange.Data.State == "Finished" || jobStateChange.Data.State == "Error"))
            {
                log.LogInformation($"jobStateChange.Data.State = {jobStateChange.Data.State}. Nothing to do.");
                return;
            }
            else
            {
                // Function input comes from the request content.
                string instanceId = await starter.StartNewAsync("VideoSummaryTrigger", eventGridEvent);

                log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
                log.LogInformation(eventGridEvent.ToString());

                //return starter.CreateCheckStatusResponse(req, instanceId);
            }

        }
    }
}