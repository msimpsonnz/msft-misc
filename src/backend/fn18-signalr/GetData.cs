
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
using System.Dynamic;
using System.Collections.Generic;

namespace fn18_signalr
{
    public static class GetData
    {
        [FunctionName("GetData")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            dynamic flight = new ExpandoObject();
            flight.id = 111;
            flight.from = "SEA";
            flight.to = "YVR";
            flight.price = 199;

            dynamic flight2 = new ExpandoObject();
            flight2.id = 112;
            flight2.from = "AKL";
            flight2.to = "WLG";
            flight2.price = 199;

            List<dynamic> flights = new List<dynamic>();
            flights.Add(flight);
            flights.Add(flight2);

            return (ActionResult)new OkObjectResult(flights);
        }
    }
}
