using Newtonsoft.Json;
using System.Collections.Generic;

namespace Portal.Shared
{
    public class PortalUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("isadmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("experience")]
        public int Experience { get; set; }

        [JsonProperty("submissions")]
        public List<Submission> Submissions { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }    

    }

}
