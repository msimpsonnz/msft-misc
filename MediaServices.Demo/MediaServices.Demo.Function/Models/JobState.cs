using Newtonsoft.Json;
using System;

namespace MediaServices.Demo.Function.Models
{
    public partial class JobStateChange
    {
        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("eventTime")]
        public DateTimeOffset EventTime { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("dataVersion")]
        public string DataVersion { get; set; }

        [JsonProperty("metadataVersion")]
        public long MetadataVersion { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("previousState")]
        public string PreviousState { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("CorrelationData")]
        public CorrelationData CorrelationData { get; set; }
    }

    public partial class CorrelationData
    {
        [JsonProperty("frame_rate")]
        public string FrameRate { get; set; }

        [JsonProperty("video_bitrate")]
        public string VideoBitrate { get; set; }

        [JsonProperty("audio_bitrate")]
        public string AudioBitrate { get; set; }

        [JsonProperty("audio_codec")]
        public string AudioCodec { get; set; }

        [JsonProperty("audio_sample_rate")]
        public string AudioSampleRate { get; set; }

        [JsonProperty("channels")]
        public string Channels { get; set; }

        [JsonProperty("duration_in_secs")]
        public string Duration { get; set; }

        [JsonProperty("width")]
        public string Width { get; set; }

        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("video_codec")]
        public string VideoCodec { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("total_bitrate")]
        public string TotalBitrate { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
