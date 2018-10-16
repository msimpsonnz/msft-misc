using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MediaServices.Demo.Function
{
    public class VideoInfo
    {

        public static async Task<Dictionary<string, string>> BlobVideoInfo(string blobUri, ILogger log)
        {
            Dictionary<string, string> blobVideoInfo = new Dictionary<string, string>();

            MetaData meta = await GetBlob(blobUri, log);

            //frame_rate = video.Framerate,
            //audio_bitrate_in_kbps = (int)audio.Bitrate, //TODO: unsafe
            //audio_codec = audio.Codec,
            // audio_sample_rate = (int)audio.SamplingRate, //TODO: unsafe
            //channels = (int)audio.Channels, //TODO: unsafe
            // duration_in_ms = ConvertDurationToMs(asset.Duration)

            blobVideoInfo.Add("frame_rate", meta.streams[1].avg_frame_rate);
            blobVideoInfo.Add("audio_bitrate_in_kbps", meta.streams[0].bit_rate);
            blobVideoInfo.Add("audio_codec ", meta.streams[0].codec_name);
            blobVideoInfo.Add("audio_sample_rate", meta.streams[0].sample_rate);
            blobVideoInfo.Add("channels", meta.streams[0].channels.ToString());
            blobVideoInfo.Add("duration_in_ms", meta.streams[0].duration);

            return blobVideoInfo;
        }

        public static async Task<MetaData> GetBlob(string blobUri, ILogger log)
        {

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = @".\util\ffprobe.exe";
                process.StartInfo.Arguments = $"-v quiet -show_streams -print_format json \"{blobUri}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string err = process.StandardError.ReadToEnd();



                process.WaitForExit();

                log.LogInformation(output);

                MetaData result = JsonConvert.DeserializeObject<MetaData>(output);

                return result;
            }

            catch (Exception ex)
            {
                log.LogError($"Error extracting metadata from Video for {blobUri}, Exception: {ex.Message}");
                throw;
            }
        }
    }



    public class MetaData
    {
        public Stream[] streams { get; set; }
    }

    public class Stream
    {
        public int index { get; set; }
        public string codec_name { get; set; }
        public string codec_long_name { get; set; }
        public string profile { get; set; }
        public string codec_type { get; set; }
        public string codec_time_base { get; set; }
        public string codec_tag_string { get; set; }
        public string codec_tag { get; set; }
        public string sample_fmt { get; set; }
        public string sample_rate { get; set; }
        public int channels { get; set; }
        public string channel_layout { get; set; }
        public int bits_per_sample { get; set; }
        public string r_frame_rate { get; set; }
        public string avg_frame_rate { get; set; }
        public string time_base { get; set; }
        public int start_pts { get; set; }
        public string start_time { get; set; }
        public int duration_ts { get; set; }
        public string duration { get; set; }
        public string bit_rate { get; set; }
        public string max_bit_rate { get; set; }
        public string nb_frames { get; set; }
        public Disposition disposition { get; set; }
        public Tags tags { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int coded_width { get; set; }
        public int coded_height { get; set; }
        public int has_b_frames { get; set; }
        public string sample_aspect_ratio { get; set; }
        public string display_aspect_ratio { get; set; }
        public string pix_fmt { get; set; }
        public int level { get; set; }
        public string chroma_location { get; set; }
        public int refs { get; set; }
        public string is_avc { get; set; }
        public string nal_length_size { get; set; }
        public string bits_per_raw_sample { get; set; }
    }

    public class Disposition
    {
        public int _default { get; set; }
        public int dub { get; set; }
        public int original { get; set; }
        public int comment { get; set; }
        public int lyrics { get; set; }
        public int karaoke { get; set; }
        public int forced { get; set; }
        public int hearing_impaired { get; set; }
        public int visual_impaired { get; set; }
        public int clean_effects { get; set; }
        public int attached_pic { get; set; }
        public int timed_thumbnails { get; set; }
    }

    public class Tags
    {
        public DateTime creation_time { get; set; }
        public string language { get; set; }
        public string handler_name { get; set; }
    }


}

