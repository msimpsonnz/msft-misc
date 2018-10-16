using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediaServices.Demo.Function
{
    public static class VideoSummary
    {
        private static readonly string AMSStorageConnectionString = Environment.GetEnvironmentVariable("AMSStorageConnectionString");
        private static readonly string DirectoryPath = Environment.GetEnvironmentVariable("WorkingDir");

        private static CloudBlobClient storageClient;
        private static CloudBlobContainer storageContainer;
        private static string workingDir;
        private static List<string> localBlobs = new List<string>();

        [FunctionName("VideoSummary_Runner")]
        public static async Task Run([ActivityTrigger] DurableActivityContext inputs, ILogger log)
        {
            log.LogInformation($"Runner Started");
            (string correlationId, string assetId) eventData = inputs.GetInput<(string, string)>();
            string jobId = eventData.correlationId;
            workingDir = Directory.CreateDirectory(DirectoryPath + jobId).FullName.ToString();

            string outputName = $"summary-{jobId}.mp4";
            var storageAccount = CloudStorageAccount.Parse(AMSStorageConnectionString);
            storageClient = storageAccount.CreateCloudBlobClient();
            storageContainer = storageClient.GetContainerReference(eventData.assetId);
            try
            {
                //MSI could be used in future when blob AAD access is out of preview
                ////Get MSI Token from Function Host
                //var azureServiceTokenProvider = new AzureServiceTokenProvider();
                //string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(ArmEndpoint);
                //ServiceClientCredentials credentials = new TokenCredentials(accessToken);
                await GetThumbnails(log);
                var trimBlobId = TrimBlobId(localBlobs[0]);
                var summaryId = FFProcess.CreateSprite(workingDir, trimBlobId, outputName, eventData.correlationId, log);
                await UploadSummary(workingDir, outputName, log);


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

        private static async Task GetThumbnails(ILogger log)
        {
            var blobList = await GetBlobList(log);
            await DownloadBlobs(blobList, log);

        }

        private static async Task UploadSummary(string summaryLocation, string summaryId, ILogger log)
        {
            log.LogInformation("Uploading summary");
            CloudBlockBlob cloudBlockBlob = storageContainer.GetBlockBlobReference(summaryId);
            await cloudBlockBlob.UploadFromFileAsync(Path.Combine(summaryLocation, summaryId));

        }

        private static async Task DownloadBlobs(BlobResultSegment thumbList, ILogger log)
        {
            log.LogInformation("Downloading blobs");
            foreach (var blobItem in thumbList.Results.Where(r => Path.GetExtension(r.Uri.ToString()) == ".png"))
            {
                log.LogInformation($"Downloading: {blobItem.Uri.ToString()}");
                CloudBlockBlob blockBlob = storageContainer.GetBlockBlobReference(((CloudBlockBlob)blobItem).Name);
                var blobPath = Path.Combine(workingDir, blockBlob.Name);
                await blockBlob.DownloadToFileAsync(blobPath, FileMode.Create);
                localBlobs.Add(blobPath);
            }
        }


        private static async Task<BlobResultSegment> GetBlobList(ILogger log)
        {
            log.LogInformation("Getting list of blobs in container");
            BlobResultSegment blobResult;
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                blobResult = await storageContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                blobContinuationToken = blobResult.ContinuationToken;
            } while (blobContinuationToken != null);
            return blobResult;
        }

    }
}
