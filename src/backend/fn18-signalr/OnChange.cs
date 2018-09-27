
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


            dynamic flight1 = new ExpandoObject();
            flight1.from = "SEA";
            flight1.to = "YVR";
            flight1.price = price;

            dynamic flight2 = new ExpandoObject();
            flight2.from = "SEA";
            flight2.to = "YVR";
            flight2.price = price;

            await signalRMessages.AddAsync(new SignalRMessage
            {
                Target = "flightUpdated",
                Arguments = new[] { flight1 }
            });

            return (ActionResult)new OkObjectResult($"Signal R");
        }
    }
}
