using System;
using Newtonsoft.Json;

namespace Common
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

       [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }

        [JsonProperty("emailaddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}