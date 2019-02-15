using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Portal.Shared;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Portal.Functions
{
    public static class Validate
    {
        static HttpClient client = new HttpClient();

        [FunctionName("OCR")]
        public async static void Run(
            [EventGridTrigger]JObject eventGridEvent,
            [CosmosDB("%Cosmos:DatabaseName%", "%Cosmos:CollectionName%", ConnectionStringSetting = "Cosmos:ConnectionString")] IAsyncCollector<dynamic> documentOut,
            ILogger log)
        {
            log.LogInformation(eventGridEvent.ToString(Formatting.Indented));

            string blobUrl = eventGridEvent["data"]["url"].ToString();
            dynamic body = new
            {
                url = blobUrl
            };
            string bodyJson = JsonConvert.SerializeObject(body);
            log.LogInformation($"Status: Extract blob: {bodyJson}");

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("OCR:Subscription"));

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var recTextUri = "https://westus.api.cognitive.microsoft.com/vision/v2.0/recognizeText?mode=Printed";

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(bodyJson);

            log.LogInformation("Status: Send Request to Vision");
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(recTextUri, new ByteArrayContent(byteData));
            }
            log.LogInformation("Status: Vison Request Process");
            string textOperationUri = response.Headers.FirstOrDefault(x => x.Key == "Operation-Location").Value.FirstOrDefault();
            log.LogInformation($"Staus: Get URL, {textOperationUri}");
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
            log.LogInformation("Status: Received Result");

            var validateScore = ScoreValidator.ExtractScore(recognitionResultData);
            var techProfileUrl = ScoreValidator.ExtractUrl(recognitionResultData);
            var onlineScore = ScoreValidator.GetOnlineProfile(techProfileUrl);
            string status;
            if (onlineScore >= validateScore)
            {
                status = "Pass";
            }
            else
            {
                status = "FAIL";
            }


            Submission document = new Submission
            {
                Id = eventGridEvent["id"].ToString(),
                Date = eventGridEvent["eventTime"].ToString(),
                UserId = Guid.NewGuid().ToString(),
                BlobUri = blobUrl,
                ValidateScore = validateScore,
                OnlineScore = onlineScore,
                Status = status ?? "Validation Failure",
                TechProfile = techProfileUrl ?? "Failed to extract profile",
                Type = "submission"
            };

            await documentOut.AddAsync(document);
        }
    }
}
