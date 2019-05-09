using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FN18.Core
{
    public static class HttpClientExtension
    {
        public static async Task<T> PostJsonCustomAsync<T>(this HttpClient sender, string requestUrl, object postData)
            where T : new()
        {
            sender.DefaultRequestHeaders.Add("Accept", "application/json");

            string stringPostData = JsonConvert.SerializeObject(postData);

            HttpContent body = new StringContent(stringPostData, Encoding.UTF8, "application/json");
            var response = await sender.PostAsync(requestUrl, body);

            string text = await response.Content.ReadAsStringAsync();

            T data = JsonConvert.DeserializeObject<T>(text);

            return data;
        }
    }
}
