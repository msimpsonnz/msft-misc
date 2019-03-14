using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Function.Demo.Auth
{
    public class Data
    {
        private static readonly string secret = Environment.GetEnvironmentVariable("JwtSecret");

        [FunctionName("Data")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",
            Route = "data/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "db",
                collectionName: "coll",
                ConnectionStringSetting = "CosmosDBConnection",
                PartitionKey = "{id}",
                Id = "{id}")]
                dynamic user,
            ILogger log, 
            [AccessToken] ClaimsPrincipal principal)
        {
            log.LogInformation($"Request received for {principal.Identity.Name}.");
            return new OkObjectResult(user.hash);
        }
    }
}
