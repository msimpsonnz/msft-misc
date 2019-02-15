using Microsoft.AspNetCore.Components;
using Portal.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Portal.Client
{
    public class UserService : IUser
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://mjsdemo.azurefd.net";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<PortalUser> GetUser(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PortalUser>> ListUsers()
        {
            var url = $"{baseUrl}/api/Users/GetAll";

            return await _httpClient.GetJsonAsync<List<PortalUser>>(url);
        }
    }
}
