using Newtonsoft.Json;

namespace Portal.Shared
{
    public class Submission
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("userid")]
        public string UserId { get; set; }

        [JsonProperty("blobUri")]
        public string BlobUri { get; set; }

        [JsonProperty("validatescore")]
        public int ValidateScore { get; set; }

        [JsonProperty("onlinescore")]
        public int OnlineScore { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("techprofile")]
        public string TechProfile { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
