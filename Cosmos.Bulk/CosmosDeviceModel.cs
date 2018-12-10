using Newtonsoft.Json;

namespace NoSQL.ConsoleApp
{
    public class CosmosDeviceModel
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
