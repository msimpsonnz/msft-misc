using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Common;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Func
{
    public static class HttpLoader
    {
        [FunctionName("HttpLoader")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var input = context.GetInput<List<User>>();

            var outputs = new List<string>();

            var batches = await context.CallActivityAsync<List<List<User>>>("HttpLoader_BatchReq", input);

            var parallelTasks = new List<Task<string>>();

            foreach (var batch in batches)
            {
                foreach (var user in batch)
                {
                        Task<string> task = context.CallActivityAsync<string>("HttpLoader_Load", user);
                        parallelTasks.Add(task);
                }

            }

            IEnumerable<string> results = await Task.WhenAll(parallelTasks);
            return results.ToList();
        }


        [FunctionName("HttpLoader_BatchReq")]
        public static async Task<IEnumerable<IEnumerable<User>>> BatchReq([ActivityTrigger] IEnumerable<User> users, ILogger log)
        {
            return BatchHelper.Batch(users, 100);


        }

        [FunctionName("HttpLoader_Load")]
        public static async Task<string> WebTest([ActivityTrigger] User user, ILogger log)
        {
            string tokenUrl = "https://mjsdemofuncauth.azurewebsites.net/api/user/";
            string dataUrl = "https://mjsdemofuncauth.azurewebsites.net/api/data/";
            var body = JsonConvert.SerializeObject(user);
            var req = HttpHelper.MakeRequest(tokenUrl+user.Id, HttpMethod.Post, body);
            var res = await HttpHelper.AuthRequest(req, dataUrl, user, body);

            return res;
        }

        [FunctionName("HttpLoader_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            string requestBody = await req.Content.ReadAsStringAsync();
            List<User> data = JsonConvert.DeserializeObject<List<User>>(requestBody);

            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("HttpLoader", data);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}