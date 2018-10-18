using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediaServices.Demo.Function.Helpers;

namespace MediaServices.Demo.Function
{
    public static class VideoSummary
    {
        private static readonly string AMSStorageConnectionString = Environment.GetEnvironmentVariable("AMSStorageConnectionString");
        private static readonly string DirectoryPath = Environment.GetEnvironmentVariable("WorkingDir");
        private static readonly string ffmpegLocation = Environment.GetEnvironmentVariable("ffmpegLocation");

        [FunctionName("VideoSummary_Runner")]
        public static async Task Run([ActivityTrigger] DurableActivityContext inputs, ILogger log)
        {
            log.LogInformation($"Runner Started");
            //ToDo - Fix getting info from Event
            (string correlationId, string assetId) eventData = inputs.GetInput<(string, string)>();
            string jobId = eventData.correlationId;
            //Create a Unique working directory
            string workingDir = Directory.CreateDirectory(DirectoryPath + jobId).FullName.ToString();
            //Create collection for list of png files
            List<string> localBlobs = new List<string>();
            //Create a unique output name for video summary file
            string outputName = $"summary-{jobId}.mp4";
            //Create a blob client to the AMS account and asset container created by the encoding Job
            BlobHelper.CreateStorageConnection(AMSStorageConnectionString, eventData.assetId);
            try
            {
                //MSI could be used in future when blob AAD access is out of preview
                ////Get MSI Token from Function Host
                //var azureServiceTokenProvider = new AzureServiceTokenProvider();
                //string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(ArmEndpoint);
                //ServiceClientCredentials credentials = new TokenCredentials(accessToken);
                //Get list of thumnails
                localBlobs = await GetThumbnails(workingDir, log);
                var trimBlobId = TrimBlobId(localBlobs[0]);
                string summaryArgs = $"-v quiet -r 1/5 -i {trimBlobId}%06d.png -c:v libx264 -vf fps=25 -pix_fmt yuv420p {outputName}";
                var summaryId = FFMpeg.RunFFMpeg(workingDir, ffmpegLocation, summaryArgs, eventData.correlationId, log);
                await BlobHelper.UploadSummary(Path.Combine(workingDir, outputName), outputName, log);


            }
            catch (Exception ex)
            {
                log.LogError($"Status: Error with Function init: {ex.Message}, CorrelationId: {eventData.correlationId}");
                throw;
            }
            finally
            {
                Directory.Delete(workingDir, true);
            }

        }

        private static string TrimBlobId(string blobId)
        {
            return blobId.Remove(blobId.LastIndexOf('-') + 1);
        }

        private static async Task<List<string>> GetThumbnails(string workingDir, ILogger log)
        {
            var blobList = await BlobHelper.GetBlobList(log);
            return await BlobHelper.DownloadBlobs(blobList, workingDir, log);

        }

    }
}
