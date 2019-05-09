using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MediaServices.Demo.Function;

namespace MediaServices.Demo.Tests
{
    [TestClass]
    public class EncodeTests
    {
        [TestMethod]
        public void FfprobeOutputSerialisation()
        {
            // arrange  
            string[] ffOutput = new[]
            {
                " { \"streams\": [ { \"index\": 0, \"codec_name\": \"h264\", \"codec_long_name\": \"H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10\", \"profile\": \"Main\", \"codec_type\": \"video\", \"codec_time_base\": \"1/50\", \"codec_tag_string\": \"[0][0][0][0]\", \"codec_tag\": \"0x0000\", \"width\": 1280, \"height\": 720, \"coded_width\": 1280, \"coded_height\": 720, \"has_b_frames\": 2, \"sample_aspect_ratio\": \"1:1\", \"display_aspect_ratio\": \"16:9\", \"pix_fmt\": \"yuv420p\", \"level\": 40, \"color_range\": \"tv\", \"color_space\": \"bt709\", \"color_transfer\": \"bt709\", \"color_primaries\": \"bt709\", \"chroma_location\": \"left\", \"field_order\": \"progressive\", \"refs\": 1, \"is_avc\": \"true\", \"nal_length_size\": \"4\", \"r_frame_rate\": \"25/1\", \"avg_frame_rate\": \"25/1\", \"time_base\": \"1/1000\", \"start_pts\": 0, \"start_time\": \"0.000000\", \"bits_per_raw_sample\": \"8\", \"disposition\": { \"default\": 1, \"dub\": 0, \"original\": 0, \"comment\": 0, \"lyrics\": 0, \"karaoke\": 0, \"forced\": 0, \"hearing_impaired\": 0, \"visual_impaired\": 0, \"clean_effects\": 0, \"attached_pic\": 0, \"timed_thumbnails\": 0 }, \"tags\": { \"language\": \"eng\" } }, { \"index\": 1, \"codec_name\": \"aac\", \"codec_long_name\": \"AAC (Advanced Audio Coding)\", \"profile\": \"LC\", \"codec_type\": \"audio\", \"codec_time_base\": \"1/44100\", \"codec_tag_string\": \"[0][0][0][0]\", \"codec_tag\": \"0x0000\", \"sample_fmt\": \"fltp\", \"sample_rate\": \"44100\", \"channels\": 2, \"channel_layout\": \"stereo\", \"bits_per_sample\": 0, \"r_frame_rate\": \"0/0\", \"avg_frame_rate\": \"0/0\", \"time_base\": \"1/1000\", \"start_pts\": 0, \"start_time\": \"0.000000\", \"disposition\": { \"default\": 1, \"dub\": 0, \"original\": 0, \"comment\": 0, \"lyrics\": 0, \"karaoke\": 0, \"forced\": 0, \"hearing_impaired\": 0, \"visual_impaired\": 0, \"clean_effects\": 0, \"attached_pic\": 0, \"timed_thumbnails\": 0 }, \"tags\": { \"language\": \"eng\" } } ] } ",
                "{\r\n    \"streams\": [\r\n    {\r\n        \"index\": 0,\r\n        \"codec_name\": \"h264\",\r\n        \"codec_long_name\": \"H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10\",\r\n        \"profile\": \"Main\",\r\n        \"codec_type\": \"video\",\r\n        \"codec_time_base\": \"1/50\",\r\n        \"codec_tag_string\": \"[0][0][0][0]\",\r\n        \"codec_tag\": \"0x0000\",\r\n        \"width\": 1280,\r\n        \"height\": 720,\r\n        \"coded_width\": 1280,\r\n        \"coded_height\": 720,\r\n        \"has_b_frames\": 2,\r\n        \"sample_aspect_ratio\": \"1:1\",\r\n        \"display_aspect_ratio\": \"16:9\",\r\n        \"pix_fmt\": \"yuv420p\",\r\n        \"level\": 40,\r\n        \"color_range\": \"tv\",\r\n        \"color_space\": \"bt709\",\r\n        \"color_transfer\": \"bt709\",\r\n        \"color_primaries\": \"bt709\",\r\n        \"chroma_location\": \"left\",\r\n        \"field_order\": \"progressive\",\r\n        \"refs\": 1,\r\n        \"is_avc\": \"true\",\r\n        \"nal_length_size\": \"4\",\r\n        \"r_frame_rate\": \"25/1\",\r\n        \"avg_frame_rate\": \"25/1\",\r\n        \"time_base\": \"1/1000\",\r\n        \"start_pts\": 0,\r\n        \"start_time\": \"0.000000\",\r\n        \"bits_per_raw_sample\": \"8\",\r\n        \"disposition\": {\r\n            \"default\": 1,\r\n            \"dub\": 0,\r\n            \"original\": 0,\r\n            \"comment\": 0,\r\n            \"lyrics\": 0,\r\n            \"karaoke\": 0,\r\n            \"forced\": 0,\r\n            \"hearing_impaired\": 0,\r\n            \"visual_impaired\": 0,\r\n            \"clean_effects\": 0,\r\n            \"attached_pic\": 0,\r\n            \"timed_thumbnails\": 0\r\n        },\r\n        \"tags\": {\r\n            \"language\": \"eng\"\r\n        }\r\n    },\r\n    {\r\n        \"index\": 1,\r\n        \"codec_name\": \"aac\",\r\n        \"codec_long_name\": \"AAC (Advanced Audio Coding)\",\r\n        \"profile\": \"LC\",\r\n        \"codec_type\": \"audio\",\r\n        \"codec_time_base\": \"1/44100\",\r\n        \"codec_tag_string\": \"[0][0][0][0]\",\r\n        \"codec_tag\": \"0x0000\",\r\n        \"sample_fmt\": \"fltp\",\r\n        \"sample_rate\": \"44100\",\r\n        \"channels\": 2,\r\n        \"channel_layout\": \"stereo\",\r\n        \"bits_per_sample\": 0,\r\n        \"r_frame_rate\": \"0/0\",\r\n        \"avg_frame_rate\": \"0/0\",\r\n        \"time_base\": \"1/1000\",\r\n        \"start_pts\": 0,\r\n        \"start_time\": \"0.000000\",\r\n        \"disposition\": {\r\n            \"default\": 1,\r\n            \"dub\": 0,\r\n            \"original\": 0,\r\n            \"comment\": 0,\r\n            \"lyrics\": 0,\r\n            \"karaoke\": 0,\r\n            \"forced\": 0,\r\n            \"hearing_impaired\": 0,\r\n            \"visual_impaired\": 0,\r\n            \"clean_effects\": 0,\r\n            \"attached_pic\": 0,\r\n            \"timed_thumbnails\": 0\r\n        },\r\n        \"tags\": {\r\n            \"language\": \"eng\"\r\n        }\r\n    }\r\n]\r\n}",
                " { \"streams\": [ { \"index\": 0, \"codec_name\": \"h264\", \"codec_long_name\": \"H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10\", \"profile\": \"Main\", \"codec_type\": \"video\", \"codec_time_base\": \"1/50\", \"codec_tag_string\": \"[0][0][0][0]\", \"codec_tag\": \"0x0000\", \"width\": 1280, \"height\": 720, \"coded_width\": 1280, \"coded_height\": 720, \"has_b_frames\": 2, \"sample_aspect_ratio\": \"1:1\", \"display_aspect_ratio\": \"16:9\", \"pix_fmt\": \"yuv420p\", \"level\": 40, \"color_range\": \"tv\", \"color_space\": \"bt709\", \"color_transfer\": \"bt709\", \"color_primaries\": \"bt709\", \"chroma_location\": \"left\", \"field_order\": \"progressive\", \"refs\": 1, \"is_avc\": \"true\", \"nal_length_size\": \"4\", \"r_frame_rate\": \"25/1\", \"time_base\": \"1/1000\", \"start_pts\": 0, \"start_time\": \"0.000000\", \"bits_per_raw_sample\": \"8\", \"disposition\": { \"default\": 1, \"dub\": 0, \"original\": 0, \"comment\": 0, \"lyrics\": 0, \"karaoke\": 0, \"forced\": 0, \"hearing_impaired\": 0, \"visual_impaired\": 0, \"clean_effects\": 0, \"attached_pic\": 0, \"timed_thumbnails\": 0 }, \"tags\": { \"language\": \"eng\" } }, { \"index\": 1, \"codec_name\": \"aac\", \"codec_long_name\": \"AAC (Advanced Audio Coding)\", \"profile\": \"LC\", \"codec_type\": \"audio\", \"codec_time_base\": \"1/44100\", \"codec_tag_string\": \"[0][0][0][0]\", \"codec_tag\": \"0x0000\", \"sample_fmt\": \"fltp\", \"sample_rate\": \"44100\", \"channels\": 2, \"channel_layout\": \"stereo\", \"bits_per_sample\": 0, \"r_frame_rate\": \"0/0\", \"avg_frame_rate\": \"0/0\", \"time_base\": \"1/1000\", \"start_pts\": 0, \"start_time\": \"0.000000\", \"disposition\": { \"default\": 1, \"dub\": 0, \"original\": 0, \"comment\": 0, \"lyrics\": 0, \"karaoke\": 0, \"forced\": 0, \"hearing_impaired\": 0, \"visual_impaired\": 0, \"clean_effects\": 0, \"attached_pic\": 0, \"timed_thumbnails\": 0 }, \"tags\": { \"language\": \"eng\" } } ] } "
            };
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            ILogger log = new Logger<NullLoggerFactory>(new NullLoggerFactory());
            string blobUri = @"https://someblob.core.windows.net";
            // act
            foreach (var o in ffOutput)
            {
                var data = VideoInfo.MetaDataResult(o);
                result.Add(VideoInfo.MapMetaData(blobUri, data, log));
            }

            //assert
            Assert.AreEqual("25/1", result[0]["frame_rate"]);
            Assert.AreEqual("25/1", result[1]["frame_rate"]);
            Assert.AreEqual("0/0", result[2]["frame_rate"]);

            var output = JsonConvert.SerializeObject(result[0]);
            System.Console.WriteLine(output);

        }
    }
}
