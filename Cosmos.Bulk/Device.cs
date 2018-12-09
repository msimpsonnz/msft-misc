using Newtonsoft.Json;

namespace Cosmos.Bulk
{
    public class Device
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "deviceid")]
        public string deviceid { get; set; }

        [JsonProperty(PropertyName = "uid")]
        public string uid { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string type { get; set; }

    }
}
