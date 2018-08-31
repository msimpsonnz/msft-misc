using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MediaServices.Demo.Function
{
    public static class HttpTest
    {
        private static readonly string testBlob = Environment.GetEnvironmentVariable("testBlob");


        [FunctionName("HttpTest")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");



           string result = await VideoInfo.GetMetadata(testBlob, log);
           //var result = MetaData.GetMeta(testBlob);


            return (ActionResult)new OkObjectResult($"{result}");
        }
    }
}
