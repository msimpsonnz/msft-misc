// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace MediaServices.Demo.Function
{
    public static class JobStatus
    {

        [FunctionName("JobStatus")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            dynamic data = eventGridEvent.Data;
            if (!(data["State"] == "Finished" || data["State"] == "Error"))
            {
                log.LogInformation($"jobStateChange.Data.State = {data["State"]}, nothting to do");
                return;
            }


        }
    }
}
