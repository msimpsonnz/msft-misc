using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Function.Demo.Auth
{
    public class VerifyUser
    {
        private static readonly string secret = Environment.GetEnvironmentVariable("JwtSecret");

        [FunctionName("VerifyUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post",
            Route = "user/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "db",
                collectionName: "coll",
                ConnectionStringSetting = "CosmosDBConnection",
                PartitionKey = "{id}",
                Id = "{id}")]
                dynamic user,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            bool authSuccess = false;
            string token = String.Empty;
            log.LogInformation($"{user.ToString()}");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);


            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: data.password.ToString(),
            salt: Convert.FromBase64String(user.salt),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
            log.LogInformation($"Computed: {hashed}");
            log.LogInformation($"Stored: {hashed}");
            if (hashed == user.hash)
            {
                authSuccess = true;
                token = Token.CreateToken(secret, user.id);
            }

            return authSuccess == true
                ? (ActionResult)new OkObjectResult(token)
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
