using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediaServices.Demo.Function.Helpers
{
    public class BlobHelper
    {
        private static CloudBlobClient storageClient;
        private static CloudBlobContainer storageContainer;

        public static void CreateStorageConnection(string storageConnectionString, string containerRef)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            storageClient = storageAccount.CreateCloudBlobClient();
            storageContainer = storageClient.GetContainerReference(containerRef);
        }

        public static async Task UploadSummary(string summaryLocation, string summaryId, ILogger log)
        {
            log.LogInformation("Uploading summary");
            CloudBlockBlob cloudBlockBlob = storageContainer.GetBlockBlobReference(summaryId);
            await cloudBlockBlob.UploadFromFileAsync(Path.Combine(summaryLocation, summaryId));

        }

        public static async Task UploadBlob(string location, string summaryId, ILogger log)
        {
            log.LogInformation("Uploading summary");
            try
            {
                CloudBlockBlob cloudBlockBlob = storageContainer.GetBlockBlobReference(summaryId);
                await cloudBlockBlob.UploadFromFileAsync(location);
            }
            catch (Exception ex)
            {
                log.LogCritical(ex.Message);
                throw;
            }

        }


        public static async Task<List<string>> DownloadBlobs(BlobResultSegment thumbList, string workingDir, ILogger log)
        {
            log.LogInformation("Downloading blobs");
            List<string> resultSet = new List<string>();
            foreach (var blobItem in thumbList.Results.Where(r => Path.GetExtension(r.Uri.ToString()) == ".png"))
            {
                log.LogInformation($"Downloading: {blobItem.Uri.ToString()}");
                CloudBlockBlob blockBlob = storageContainer.GetBlockBlobReference(((CloudBlockBlob)blobItem).Name);
                var blobPath = Path.Combine(workingDir, blockBlob.Name);
                await blockBlob.DownloadToFileAsync(blobPath, FileMode.Create);
                resultSet.Add(blobPath);
            }
            return resultSet;
        }


        public static async Task<BlobResultSegment> GetBlobList(ILogger log)
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
