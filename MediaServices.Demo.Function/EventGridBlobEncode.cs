// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGridExtensionConfig?functionName={functionname}

//Ported Media Services functions from 
//https://github.com/Azure-Samples/media-services-v3-dotnet-tutorials/blob/master/AMSV3Tutorials/UploadEncodeAndStreamFiles/Program.cs
// Credit to @Juliako, @johndeu and @mconverti

using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace MediaServices.Demo.Function
{
    public static class EventGridBlobEncode
    {
        private static readonly string ArmEndpoint = Environment.GetEnvironmentVariable("ArmEndpoint");
        private static readonly string SubscriptionId = Environment.GetEnvironmentVariable("SubscriptionId");
        private static readonly string ResourceGroup = Environment.GetEnvironmentVariable("resourceGroup");
        private static readonly string AccountName = Environment.GetEnvironmentVariable("accountName");
        private const string TransformName = "TransformAdaptiveStreaming";
        private static readonly string inputBlobSAS = Environment.GetEnvironmentVariable("inputBlobSAS");

        [FunctionName("EventGridBlobEncode")]
        public static async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, TraceWriter log)
        {
            log.Info(eventGridEvent.Data.ToString());
            //Build variables to ensure AMS Assests and Jobs are unique
            string uniqueness = Guid.NewGuid().ToString("N");
            string inputAssetName = $"input-{uniqueness}";
            string jobName = $"job-{uniqueness}";
            string outputAssetName = $"output-{uniqueness}";

            try
            {
                //Get MSI Token from Function Host
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(ArmEndpoint);
                ServiceClientCredentials credentials = new TokenCredentials(accessToken);

                //Create Media Services Client
                IAzureMediaServicesClient client = CreateMediaServicesClientAsync(credentials);

                //Ensuure Transform Exists
                Transform transform = await GetOrCreateTransformAsync(client, TransformName);

                //Create output assest for the jon
                Asset outputAsset = await CreateOutputAssetAsync(client, outputAssetName);

                //Parse Blob URI from Event data to pass to AMS Job
                var eventData = JToken.Parse(eventGridEvent.Data.ToString());

                //Submit AMS Job
                await SubmitJobAsync(eventData["url"].ToString(), outputAssetName, client, jobName, log);

            }
            catch (Exception ex)
            {
                log.Error($"Error with Function init: {ex.Message}");
                throw;
            }

        }

        private static async Task<Job> SubmitJobAsync(string inputBlobName, string outputAssetName, IAzureMediaServicesClient client, string jobName, TraceWriter log)
        {

            try
            {
                JobInputHttp jobInput = new JobInputHttp(files: new[] { $"{inputBlobName}?{inputBlobSAS}" });
                JobOutput[] jobOutputs = { new JobOutputAsset(outputAssetName) };


                // In this example, we are assuming that the job name is unique.
                //
                // If you already have a job with the desired name, use the Jobs.Get method
                // to get the existing job. In Media Services v3, Get methods on entities returns null 
                // if the entity doesn't exist (a case-insensitive check on the name).
                Job job = await client.Jobs.CreateAsync(
                    ResourceGroup,
                    AccountName,
                    TransformName,
                    jobName,
                    new Job
                    {
                        Input = jobInput,
                        Outputs = jobOutputs,
                    });

                return job;
            }
            catch (Exception ex)
            {
                log.Error($"Error with creating Job: {ex.Message}");
                throw;
            }


        }

        private static async Task<Asset> CreateOutputAssetAsync(IAzureMediaServicesClient client, string assetName)
        {
            // Check if an Asset already exists
            Asset outputAsset = await client.Assets.GetAsync(ResourceGroup, AccountName, assetName);
            Asset asset = new Asset();
            string outputAssetName = assetName;

            if (outputAsset != null)
            {
                // Name collision! In order to get the sample to work, let's just go ahead and create a unique asset name
                // Note that the returned Asset can have a different name than the one specified as an input parameter.
                // You may want to update this part to throw an Exception instead, and handle name collisions differently.
                string uniqueness = $"-{Guid.NewGuid().ToString("N")}";
                outputAssetName += uniqueness;
            }

            return await client.Assets.CreateOrUpdateAsync(ResourceGroup, AccountName, outputAssetName, asset);
        }

        private static async Task<Transform> GetOrCreateTransformAsync(IAzureMediaServicesClient client, string transformName)
        {
            // Does a Transform already exist with the desired name? Assume that an existing Transform with the desired name
            // also uses the same recipe or Preset for processing content.
            Transform transform = await client.Transforms.GetAsync(ResourceGroup, AccountName, transformName);

            if (transform == null)
            {
                // You need to specify what you want it to produce as an output
                TransformOutput[] output = new TransformOutput[]
                {
                    new TransformOutput
                    {
                        // The preset for the Transform is set to one of Media Services built-in sample presets.
                        // You can  customize the encoding settings by changing this to use "StandardEncoderPreset" class.
                        Preset = new BuiltInStandardEncoderPreset()
                        {
                            // This sample uses the built-in encoding preset for Adaptive Bitrate Streaming.
                            PresetName = EncoderNamedPreset.AdaptiveStreaming
                        }
                    }
                };

                // Create the Transform with the output defined above
                transform = await client.Transforms.CreateOrUpdateAsync(ResourceGroup, AccountName, transformName, output);
            }

            return transform;
        }

        private static IAzureMediaServicesClient CreateMediaServicesClientAsync(ServiceClientCredentials credentials)
        {
            return new AzureMediaServicesClient(new Uri(ArmEndpoint), credentials)
            {
                SubscriptionId = SubscriptionId,
            };
        }
    }

}
