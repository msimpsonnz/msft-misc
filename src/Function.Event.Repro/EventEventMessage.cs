// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace Function_Event_Repro
{
    public static class EventEventMessage
    {
        private static HttpClient _httpClient = new HttpClient();

        [FunctionName("EventMessage")]
        public static async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            string body = null;
            log.LogInformation(eventGridEvent.Data.ToString());
            log.LogInformation("this is EventGridEvent");
            try
            {
                body = await SendNotification(_httpClient, log);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                //return body; //new BadRequestResult();
            }

        }


        public static async Task<string> SendNotification(HttpClient client, ILogger log)
        {
            string body = null;
            try
            {
                var uri = new Uri($"http://localhost:7071/api/fail");
                var content = new StringContent("{{ \"Key:\" \"Value\" }}", System.Text.Encoding.UTF8, "application/json");
                log.LogInformation($"Post notifcation back to {uri}");
                var response = await client.PostAsync(uri, content);
                body = await response?.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                log.LogInformation($"Success notifcation back to {uri}, Response {response.StatusCode}");
                return body;
            }
            catch (HttpRequestException ex)
            {
                log.LogError(body ?? ex.Message, ex);
                throw;
            }
        }
    }
}
