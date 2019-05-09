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

namespace Portal.Functions
{
    public static class SubmissionFunctions
    {
        [FunctionName("GetSubmissionById")]
        public static async Task<IActionResult> GetById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Submissions/GetById")] HttpRequest req,
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

        [FunctionName("GetSubmissionAll")]
        public static async Task<IActionResult> GetStatus(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Submissions/GetAll")] HttpRequest req,
            [CosmosDB("%Cosmos:DatabaseName%", "%Cosmos:CollectionName%", ConnectionStringSetting = "Cosmos:ConnectionString")] DocumentClient client,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            var query = client.CreateDocumentQuery<Submission>(
                    UriFactory.CreateDocumentCollectionUri(Environment.GetEnvironmentVariable("Cosmos:DatabaseName"), Environment.GetEnvironmentVariable("Cosmos:CollectionName")), queryOptions)
                    .Where(f => f.Type == "submission")
                    .AsDocumentQuery();

            var results = new List<Submission>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<Submission>());
            }

            return (ActionResult)new OkObjectResult(results);
        }

        [FunctionName("GetBlobSaS")]
        //public static async Task<IActionResult> Run(
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Submissions/GetBlobSaS")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string sas = Environment.GetEnvironmentVariable("Sas");

            return (ActionResult)new OkObjectResult(sas);
        }
    }
}
