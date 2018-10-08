
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
using FN18.Core;

namespace FN18.Functions
{
    public static class Moderator
    {
        public static ModeratorService _moderatorService = new ModeratorService();

        [FunctionName("Moderator")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //req.Body
            var result = await _moderatorService.GetModeratorClient(req.Body);

            return (ActionResult)new OkObjectResult(result);
        }
    }
}
