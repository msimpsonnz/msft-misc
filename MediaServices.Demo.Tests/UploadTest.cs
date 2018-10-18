using MediaServices.Demo.Function;
using MediaServices.Demo.Function.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MediaServices.Demo.Tests
{
    [TestClass]
    public class UploadTests
    {
        private IConfiguration config;
        private static ILogger log = new Logger<NullLoggerFactory>(new NullLoggerFactory());
        private static string newVideoId = $"video-{Guid.NewGuid().ToString()}.mp4";

        [TestCategory("Integration")]
        [TestMethod]
        public async Task UploadRandomBlob()
        {
            //arrange

            string videoFile = config.GetValue<string>("videoFile");

            //act
            BlobHelper.CreateStorageConnection(config.GetValue<string>("rawStorageConnectionString"), config.GetValue<string>("rawContainerRef"));
            await BlobHelper.UploadSummary(videoFile, newVideoId, log);
            var getBlobList = await BlobHelper.GetBlobList(log);
            var res = getBlobList.Results.Where(x => ((CloudBlockBlob)x).Name == newVideoId).FirstOrDefault();

            // assert
            Assert.AreEqual(newVideoId, ((CloudBlockBlob)res).Name);

        }

        [TestInitialize]
        public void Init()
        {
            config = TestHelper.GetIConfigurationRoot();
            Environment.SetEnvironmentVariable("rawStorageConnectionString", config.GetValue<string>("rawStorageConnectionString"));
            Environment.SetEnvironmentVariable("WorkingDir", config.GetValue<string>("WorkingDir"));
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            //BlobHelper.CreateStorageConnection(config.GetValue<string>("rawStorageConnectionString"), config.GetValue<string>("rawContainerRef"));
            //var getBlobList = await BlobHelper.GetBlobList(log);
            //await BlobHelper.DeleteBlobs(getBlobList.Results.Where(x => ((CloudBlockBlob)x).Name != newVideoId), log);

        }
    }
}
