
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FN18.Functions
{
    public static class Moderator
    {
        [FunctionName("Moderator")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var result = JsonConvert.DeserializeObject<ModeratorResult>(rawResult);

            return new OkObjectResult(result);


        }

        [FunctionName("ModeratorList")]
        public static IActionResult Run1([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var result = JsonConvert.DeserializeObject<ModeratorResult>(rawResult);
            List<ModeratorResult> res = new List<ModeratorResult>();
            res.Add(result);
            res.Add(result);

            return new OkObjectResult(res);


        }


        static string rawResult = "{\r\n    \"OriginalText\": \"this is shit\",\r\n    \"NormalizedText\": \"this is shit\",\r\n    \"AutoCorrectedText\": \"this is shit\",\r\n    \"Misrepresentation\": [],\r\n    \"Classification\": {\r\n        \"Category1\": {\r\n            \"Score\": 0.099932014942169189\r\n        },\r\n        \"Category2\": {\r\n            \"Score\": 0.20339104533195496\r\n        },\r\n        \"Category3\": {\r\n            \"Score\": 0.84241998195648193\r\n        },\r\n        \"ReviewRecommended\": true\r\n    },\r\n    \"Status\": {\r\n        \"Code\": 3000,\r\n        \"Description\": \"OK\",\r\n        \"Exception\": null\r\n    },\r\n    \"PII\": {\r\n        \"Email\": [],\r\n        \"SSN\": [],\r\n        \"IPA\": [],\r\n        \"Phone\": [],\r\n        \"Address\": []\r\n    },\r\n    \"Language\": \"eng\",\r\n    \"Terms\": [\r\n        {\r\n            \"Index\": 8,\r\n            \"OriginalIndex\": 8,\r\n            \"ListId\": 0,\r\n            \"Term\": \"shit\"\r\n        }\r\n    ],\r\n    \"TrackingId\": \"bdee42cb-c588-4e33-96f9-efef22e72e7b\"\r\n}";
    }


}
