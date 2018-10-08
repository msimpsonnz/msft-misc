
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

            string json = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic content = JsonConvert.DeserializeObject<dynamic>(json);

            var result = await _moderatorService.ScoreText(content["Description"].ToString());

            return (ActionResult)new OkObjectResult(result);
        }
    }
}
