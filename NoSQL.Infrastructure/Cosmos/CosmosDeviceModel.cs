using Newtonsoft.Json;

namespace NoSQL.Infrastructure
{
    public class CosmosDeviceModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "device")]
        public string Device { get; set; }

        [JsonProperty(PropertyName = "uid")]
        public string Uid { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

    }
}
