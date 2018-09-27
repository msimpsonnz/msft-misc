
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
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB("db", "coll", ConnectionStringSetting = "AzureWebJobsCosmosDBConnectionString", SqlQuery = "SELECT top 2 * FROM c")] IEnumerable<object> docs,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult(docs);
        }
    }
}
