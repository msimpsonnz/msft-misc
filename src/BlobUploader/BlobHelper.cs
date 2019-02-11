using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlobUploader.Helpers
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
            try
            {
                CloudBlockBlob cloudBlockBlob = storageContainer.GetBlockBlobReference(summaryId);
                await cloudBlockBlob.UploadFromFileAsync(summaryLocation);
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

        public static async Task DeleteBlobs(BlobResultSegment blobList, ILogger log)
        {
            //if (blobList.Results.Count() > 1)
            //{
                log.LogInformation("Getting list of blobs in container");
                foreach (var blobItem in blobList.Results)
                {
                    CloudBlockBlob blockBlob = storageContainer.GetBlockBlobReference(((CloudBlockBlob)blobItem).Name);
                    await blockBlob.DeleteAsync();
                    log.LogInformation($"Deleted blob {((CloudBlockBlob)blobItem).Name}");
                }
            //}
        }
    }
}
