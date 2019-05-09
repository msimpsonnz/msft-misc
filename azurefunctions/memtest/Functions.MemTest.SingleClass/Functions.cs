using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Functions.MemTest.Common;

namespace Functions.MemTest.SingleClass
{
    public static class Functions
    {
        [FunctionName("Function1")]
        public static IActionResult Run1(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            AllocateMemory.GetEmptyByteArray();

            return new OkResult();
        }

        [FunctionName("Function2")]
        public static IActionResult Run2(
    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            AllocateMemory.GetEmptyByteArray();

            return new OkResult();
        }

        [FunctionName("Function3")]
        public static IActionResult Run3(
    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            AllocateMemory.GetEmptyByteArray();

            return new OkResult();
        }

        [FunctionName("Function4")]
        public static IActionResult Run4(
    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            AllocateMemory.GetEmptyByteArray();

            return new OkResult();
        }

        [FunctionName("Function5")]
        public static IActionResult Run5(
    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            AllocateMemory.GetEmptyByteArray();

            return new OkResult();
        }
    }
}
