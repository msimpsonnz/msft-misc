using FN18.Blazor.Client.Models;
using FN18.Blazor.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FN18.Blazor.Client.Extensions
{
    public class HttpApiClient : HttpClient
    {
        public HttpApiClient()
        {
            //BaseAddress = new Uri("http://localhost:7777/api/");
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
            DefaultRequestHeaders.Add("Access-Control-Allow-Methods", "*");
            DefaultRequestHeaders.Add("Access-Control-Allow-Headers", "*");
            DefaultRequestHeaders.Add("Access-Control-Max-Age", "86400");
        }
    }

    public static class HttpClientExtensions
    {

            public static async Task<ApiObjectResponse<TObject>> ApiGetAsync<TObject>(
                this HttpClient httpClient,
                string requestUri,
                Dictionary<string, string> query = null) where TObject : new()
            {
                var result = await httpClient.GetAsync(QueryHelpers.AddQueryString(requestUri, query));

                TObject resultResponse = default(TObject);
                ApiError error = null;

                if (result.IsSuccessStatusCode)
                {
                    var a = await result.Content.ReadAsStringAsync();
                    Console.WriteLine(a);
                    resultResponse = JsonConvert.DeserializeObject<TObject>(a); ;
                }
                else
                {
                    error = JsonConvert.DeserializeObject<ApiError>(await result.Content.ReadAsStringAsync()); ;
                }

                return new ApiObjectResponse<TObject>
                {
                    Error = error,
                    Response = resultResponse,
                    StatusCode = result.StatusCode
                };
            }

            public static async Task<ApiObjectResponse<TObject>> ApiPostAsync<TObject>(this HttpClient httpClient, string requestUri, object content) where TObject : new()
            {
                var myContent = JsonConvert.SerializeObject(content);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                byteContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("charset", "utf-8"));

                var result = await httpClient.PostAsync(requestUri, byteContent);

                TObject resultResponse = default(TObject);
                ApiError error = null;

                if (result.IsSuccessStatusCode)
                {
                    var a = await result.Content.ReadAsStringAsync();
                    resultResponse = JsonConvert.DeserializeObject<TObject>(a);
                }
                else
                {
                    error = JsonConvert.DeserializeObject<ApiError>(await result.Content.ReadAsStringAsync()); ;
                }

                return new ApiObjectResponse<TObject>
                {
                    Error = error,
                    Response = resultResponse,
                    StatusCode = result.StatusCode
                };
            }

            public static async Task<ApiResponse> ApiUpdateAsync(this HttpClient httpClient, string requestUri, object content)
            {
                var myContent = JsonConvert.SerializeObject(content);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                byteContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("charset", "utf-8"));

                var result = await httpClient.PutAsync(requestUri, byteContent);

                ApiError error = null;

                if (!result.IsSuccessStatusCode)
                {
                    error = JsonConvert.DeserializeObject<ApiError>(await result.Content.ReadAsStringAsync()); ;
                }

                return new ApiResponse
                {
                    Error = error,
                    StatusCode = result.StatusCode
                };
            }


            public static async Task<ApiResponse> ApiDeleteAsync(this HttpClient httpClient, string requestUri)
            {
                var result = await httpClient.DeleteAsync(requestUri);

                ApiError error = null;

                if (!result.IsSuccessStatusCode)
                {
                    error = JsonConvert.DeserializeObject<ApiError>(await result.Content.ReadAsStringAsync()); ;
                }

                return new ApiResponse
                {
                    Error = error,
                    StatusCode = result.StatusCode
                };
            }
        }
    public static class QueryHelpers
    {
        public static string AddQueryString(string uri, Dictionary<string, string> query)
        {
            if (query == null) return uri;
            var stringBuilder = new StringBuilder();
            string str = "?";
            foreach (var q in query)
            {
                stringBuilder.Append(str + q.Key + "=" + q.Value);
                str = "&";
            }
            return (uri + stringBuilder.ToString());
        }

    }
}
