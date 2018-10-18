using MediaServices.Demo.Function;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
<<<<<<< HEAD
using MediaServices.Demo.Function.Models;
=======
>>>>>>> 3798f69cf0aa01560cf0a1f0eb2bd4ce5d6f7293

namespace MediaServices.Demo.Tests
{
    [TestClass]
    public class MetadataTests
    {
        private IConfiguration config;
        private MetaData rawMetaData;
        
        [TestCategory("Unit")]
        [TestMethod]
<<<<<<< HEAD
        public void GetBlob_DoesNotError()
=======
        public async Task GetBlob_DoesNotError()
>>>>>>> 3798f69cf0aa01560cf0a1f0eb2bd4ce5d6f7293
        {
            //arrange
            ILogger log = new Logger<NullLoggerFactory>(new NullLoggerFactory());
            string videoFile = config.GetValue<string>("videoFile");
            string correlationId = Guid.NewGuid().ToString();
            //act
<<<<<<< HEAD
            var testResult = VideoInfo.GetBlob(videoFile, log); 
=======
            var testResult = await VideoInfo.GetBlob(videoFile, log); 
>>>>>>> 3798f69cf0aa01560cf0a1f0eb2bd4ce5d6f7293
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
