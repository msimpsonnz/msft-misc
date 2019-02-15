using Microsoft.AspNetCore.Components;
using Portal.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Portal.Client
{
    public class SubmissionService : ISubmission
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://mjsdemo.azurefd.net";

        public SubmissionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetBlobSas()
        {
            var url = $"{baseUrl}/api/Submissions/GetBlobSas";

            return await _httpClient.GetStringAsync(url);
        }

        public Task<Submission> GetSubmission(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Submission>> ListSubmissions()
        {
            var url = $"{baseUrl}/api/Submissions/GetAll";

            return await _httpClient.GetJsonAsync<List<Submission>>(url);
        }


    }
}
