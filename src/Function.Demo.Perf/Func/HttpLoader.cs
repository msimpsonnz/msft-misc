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

            //// Replace "hello" with the name of your Durable Activity Function.
            //outputs.Add(await context.CallActivityAsync<string>("HttpLoader_Hello", "Tokyo"));
            //outputs.Add(await context.CallActivityAsync<string>("HttpLoader_Hello", "Seattle"));
            //outputs.Add(await context.CallActivityAsync<string>("HttpLoader_Hello", "London"));

            //// returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            //return outputs;
            var tasks = new Task<bool>[input.Count];
            foreach (User u in input)
            {
                outputs.Add(await context.CallActivityAsync<string>("HttpLoader_Load", u));
            }

            //await Task.WhenAll(tasks);
            return outputs;


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