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
        public string ValidateScore { get; set; }

        [JsonProperty("onlinescore")]
        public string OnlineScore { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

    }
}
