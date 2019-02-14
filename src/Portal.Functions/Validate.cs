using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Web;
using System.Linq;
using Portal.Shared;
using Microsoft.Azure.Documents.Client;

namespace Portal.Functions
{
    public static class Validate
    {
        static HttpClient client = new HttpClient();

        [FunctionName("OCR")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB("%Cosmos:DatabaseName%", "%Cosmos:CollectionName%", ConnectionStringSetting = "Cosmos:ConnectionString")] IAsyncCollector<dynamic> documentOut,
            ILogger log)
        {
            string body = "{\"url\":\"https://mjsdemofrontaue.blob.core.windows.net/uploads/4.png\"}";

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("OCR:Subscription"));

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var recTextUri = "https://westus.api.cognitive.microsoft.com/vision/v2.0/recognizeText?mode=Printed";

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(body);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(recTextUri, new ByteArrayContent(byteData));
            }
            string textOperationUri = response.Headers.FirstOrDefault(x => x.Key == "Operation-Location").Value.FirstOrDefault();

            HttpResponseMessage textOperationReq;

            textOperationReq = await client.GetAsync(textOperationUri);
            var textOperationRes = await textOperationReq.Content.ReadAsStringAsync();
            RecognitionResult recognitionResultData = JsonConvert.DeserializeObject<RecognitionResult>(textOperationRes);
            while (recognitionResultData.status != "Succeeded")
            {
                await Task.Delay(1000);
                textOperationReq = await client.GetAsync(textOperationUri);
                textOperationRes = await textOperationReq.Content.ReadAsStringAsync();
                recognitionResultData = JsonConvert.DeserializeObject<RecognitionResult>(textOperationRes);
            }
            RecognitionResult recognitionResult = new RecognitionResult();
            var result = recognitionResult.GetXP(recognitionResultData);

            dynamic document = new
            {
                id = Guid.NewGuid(),
                currentXP = result
            };

            await documentOut.AddAsync(document);

            return (ActionResult)new OkObjectResult(result);
        }
    }
}
