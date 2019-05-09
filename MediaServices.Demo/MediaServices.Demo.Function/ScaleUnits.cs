// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using MediaServices.Demo.Function.Helpers;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Rest;

namespace MediaServices.Demo.Function
{
    public static class ScaleUnits
    {
        private static HttpClient httpClient = new HttpClient();

        [FunctionName("ScaleUnits")]
        public static async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            dynamic data = eventGridEvent.Data;
            if (data["state"] != "Processing")
            {
                log.LogInformation($"jobStateChange.Data.State = {data["state"]}, nothting to do");
                return;
            }
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://rest.media.azure.net");

            //var req = await MediaServicesHelper.ScaleUpReservedUnits(httpClient, accessToken);
            //log.LogInformation($"Set Reserved Units to {req.value[0].CurrentReservedUnits}");

            var req = await MediaServicesHelper.ScaleDownReservedUnits(httpClient, accessToken);
            log.LogInformation($"Set Reserved Units to {req.value[0].CurrentReservedUnits}");

        }
    }
}
