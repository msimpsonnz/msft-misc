using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Portal.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Functions.Submission
{
    public static class UserStatus
    {
        [FunctionName("GetStatusById")]
        public static async Task<IActionResult> GetStatusById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [CosmosDB("%Cosmos:DatabaseName%", "%Cosmos:CollectionName%", ConnectionStringSetting = "Cosmos:ConnectionString")] DocumentClient client,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string userid = req.Query["userid"];

            Uri docUri = UriFactory.CreateDocumentUri("%Cosmos:DatabaseName%", "%Cosmos:CollectionName%", userid);
            Document doc = await client.ReadDocumentAsync(docUri);

            var res = JsonConvert.SerializeObject(doc, Formatting.None);

            return userid != null
                ? (ActionResult)new OkObjectResult(res)
                : new BadRequestObjectResult("Please pass a userid on the query string");
        }

        [FunctionName("GetUsers")]
        public static async Task<IActionResult> GetStatus(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/GetUsers")] HttpRequest req,
            [CosmosDB("%Cosmos:DatabaseName%", "%Cosmos:CollectionName%", ConnectionStringSetting = "Cosmos:ConnectionString")] DocumentClient client,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            var userQuery = client.CreateDocumentQuery<PortalUser>(
                    UriFactory.CreateDocumentCollectionUri(Environment.GetEnvironmentVariable("Cosmos:DatabaseName"), Environment.GetEnvironmentVariable("Cosmos:CollectionName")), queryOptions)
                    .Where(f => f.Type == "user")
                    .AsDocumentQuery();

            var results = new List<PortalUser>();
            while (userQuery.HasMoreResults)
            {
                results.AddRange(await userQuery.ExecuteNextAsync<PortalUser>());
            }

            return (ActionResult)new OkObjectResult(results);
        }
    }
}
