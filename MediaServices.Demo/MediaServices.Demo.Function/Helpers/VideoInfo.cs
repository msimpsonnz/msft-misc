using MediaServices.Demo.Function.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MediaServices.Demo.Function
{
    public class VideoInfo
    {
        private static readonly string ffprobeLocation = Environment.GetEnvironmentVariable("ffprobeLocation");
        private static readonly string DirectoryPath = Environment.GetEnvironmentVariable("WorkingDir");

        public static Dictionary<string, string> BlobVideoInfo(string blobUri, string correlationId, ILogger log)
        {
            MetaData meta = GetBlob(blobUri, correlationId, log);
            return MapMetaData(blobUri, meta, log);
        }
        public static Dictionary<string, string> MapMetaData(string blobUri, MetaData rawMetaData, ILogger log)
        {
            Dictionary<string, string> blobVideoInfo = new Dictionary<string, string>();

            Stream audioStream = rawMetaData.streams
                .Where(s => s.codec_type == "audio")
                .FirstOrDefault();

            Stream videoStream = rawMetaData.streams
                .Where(s => s.codec_type == "video")
                .FirstOrDefault();

            int videoBitRate, audioBitRate;
            int.TryParse(videoStream.bit_rate, out videoBitRate);
            int.TryParse(audioStream.bit_rate, out audioBitRate);
            string total_bitrate = (videoBitRate + audioBitRate).ToString();

            blobVideoInfo.Add("frame_rate", videoStream.avg_frame_rate ?? "0/0");
            blobVideoInfo.Add("duration_in_secs", videoStream.duration);
            blobVideoInfo.Add("width", videoStream.width.ToString());
            blobVideoInfo.Add("height", videoStream.height.ToString());
            blobVideoInfo.Add("video_codec", videoStream.codec_name);
            blobVideoInfo.Add("format", videoStream.codec_type);
            blobVideoInfo.Add("total_bitrate", total_bitrate);
            blobVideoInfo.Add("video_bitrate", videoStream.bit_rate);
            blobVideoInfo.Add("audio_bitrate", audioStream.bit_rate);
            blobVideoInfo.Add("audio_codec ", audioStream.codec_name);
            blobVideoInfo.Add("audio_sample_rate", audioStream.sample_rate);
            blobVideoInfo.Add("channels", audioStream.channels.ToString());
            blobVideoInfo.Add("url", blobUri);

            return blobVideoInfo;
        }

        public static MetaData GetBlob(string blobUri, string correlationId, ILogger log)
        {
            try
            {
                string probeArgs = $"-v quiet -show_streams -print_format json \"{blobUri}\"";
                var output = FFMpeg.RunFFMpeg(DirectoryPath, ffprobeLocation, probeArgs, correlationId, log);

                MetaData result = JsonConvert.DeserializeObject<MetaData>(output);

                return result;

            }

            catch (Exception ex)
            {
                log.LogError($"Error extracting metadata from Video for {blobUri}, Exception: {ex.Message}");
                throw;
            }
        }

        public static MetaData MetaDataResult(string ffprobeOutput)
        {
            return JsonConvert.DeserializeObject<MetaData>(ffprobeOutput);
        }
    }
}

