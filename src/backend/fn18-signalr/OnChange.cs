
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Collections.Generic;
using System.Dynamic;

namespace fn18_signalr
{
    public static class OnChange
    {
        [FunctionName("OnChange")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [SignalR(HubName = "flights")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int price;
            string queryPrice = req.Query["Price"];
            int.TryParse(queryPrice, out price);
            string queryFrom = req.Query["From"];
            dynamic flight = new ExpandoObject();
            if (queryFrom == "SEA")
            {
            flight.id = "852b0995-e245-4b28-f4ea-5343b4eb9525";
            flight.from = "SEA";
            flight.to = "YVR";
            flight.price = price;
            }
            else
            {
            flight.id = "386eae79-8cb7-1df3-87b4-4cba67ebfadb";
            flight.from = "AKL";
            flight.to = "CHC";
            flight.price = price;
            }


            await signalRMessages.AddAsync(new SignalRMessage
            {
                Target = "flightUpdated",
                Arguments = new[] { flight }
            });

            return (ActionResult)new OkObjectResult($"Signal R");
        }
    }
}
