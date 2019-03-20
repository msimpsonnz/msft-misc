using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Common
{
    public class HttpHelper
    {
        private static HttpClient client = new HttpClient();

        public static HttpRequestMessage MakeRequest(string uri, HttpMethod method, string body = null, JArray headers = null, string token = null)
        {
            var request = new HttpRequestMessage(method, uri);

            if (body != null)
            {
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            }

            if (headers != null)
            {
                foreach (var h in headers)
                {
                    switch (h["key"].ToString())
                    {
                        case "Content-Type":
                            break;

                        default:
                            client.DefaultRequestHeaders.Add(h["key"].ToString(), h["value"].ToString());
                            break;
                    }
                }
            }

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return request;
        }

        public static async Task<string> SendRequest(HttpRequestMessage request)
        {
            var req = await client.SendAsync(request);
            Console.WriteLine(req.StatusCode);
            return req.StatusCode.ToString();

        }

        public static async Task SendRequest(List<HttpRequestMessage> requests)
        {
            foreach (var request in requests)
            {
                var req = await client.SendAsync(request);
                Console.WriteLine(req.StatusCode);
            }

        }

        public static async Task<string> AuthRequest(HttpRequestMessage request, string dataUrl, User user, string content)
        {
            var authReq = await client.SendAsync(request);
            var authResponse = await authReq.Content.ReadAsStringAsync();
            string url = $"{dataUrl}{user.Id}";
            var datareq = MakeRequest(url, HttpMethod.Get, content, null, authResponse);
            var dataResponse = await SendRequest(datareq);
            return dataResponse;
        }

    }
}
