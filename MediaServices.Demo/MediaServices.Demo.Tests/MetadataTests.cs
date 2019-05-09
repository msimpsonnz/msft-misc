using MediaServices.Demo.Function;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using MediaServices.Demo.Function.Models;

namespace MediaServices.Demo.Tests
{
    [TestClass]
    public class MetadataTests
    {
        private IConfiguration config;
        private MetaData rawMetaData;
        
        [TestCategory("Unit")]
        [TestMethod]
        public void GetBlob_DoesNotError()
        {
            //arrange
            ILogger log = new Logger<NullLoggerFactory>(new NullLoggerFactory());
            string videoFile = config.GetValue<string>("videoFile");
            string correlationId = Guid.NewGuid().ToString();
            //act
            var testResult = VideoInfo.GetBlob(videoFile, correlationId, log); 
            // assert
            Assert.AreEqual(rawMetaData.streams[0].avg_frame_rate, testResult.streams[0].avg_frame_rate);
            Assert.AreEqual(rawMetaData.streams[0].codec_name, testResult.streams[0].codec_name);

        }

        [TestInitialize]
        public void Init()
        {
            config = TestHelper.GetIConfigurationRoot();
            Environment.SetEnvironmentVariable("ffprobeLocation", config.GetValue<string>("ffprobeLocation"));
            rawMetaData = TestHelper.GetApplicationConfiguration();
        }
    }
}
