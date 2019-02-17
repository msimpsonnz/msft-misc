using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;

namespace Portal.Functions
{
    public static class SignalRInfoFunctions
    {
        [FunctionName("SignalRInfo")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
            [SignalRConnectionInfo(HubName = "submissions")] SignalRConnectionInfo connectionInfo,
            ILogger log)
        {
            return new OkObjectResult(connectionInfo);
        }
    }
}
