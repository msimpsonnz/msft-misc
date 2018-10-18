using MediaServices.Demo.Function;
using MediaServices.Demo.Function.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MediaServices.Demo.Tests
{
    [TestClass]
    public class UploadTests
    {
        private IConfiguration config;

        [TestCategory("Unit")]
        [TestMethod]
        public async Task UploadRandomBlob()
        {
            //arrange
            ILogger log = new Logger<NullLoggerFactory>(new NullLoggerFactory());
            string videoFile = config.GetValue<string>("videoFile");
            string newVideoId = $"video-{Guid.NewGuid().ToString()}.mp4";

            //act
            BlobHelper.CreateStorageConnection(config.GetValue<string>("rawStorageConnectionString"), config.GetValue<string>("rawContainerRef"));
            await BlobHelper.UploadBlob(videoFile, newVideoId, log);
            var getBlobList = await BlobHelper.GetBlobList(log);

            // assert
            Assert.AreEqual(newVideoId, getBlobList.Results.Where(x => x.Uri.LocalPath == newVideoId).FirstOrDefault());
            Console.WriteLine();
        }

        [TestInitialize]
        public void Init()
        {
            config = TestHelper.GetIConfigurationRoot();
            Environment.SetEnvironmentVariable("rawStorageConnectionString", config.GetValue<string>("rawStorageConnectionString"));
            Environment.SetEnvironmentVariable("WorkingDir", config.GetValue<string>("WorkingDir"));
        }
    }
}
