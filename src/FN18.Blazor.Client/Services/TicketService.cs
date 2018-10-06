using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FN18.Blazor.Client.Extensions;
using FN18.Blazor.Client.Models;
using FN18.Blazor.Shared.Models;
using Microsoft.AspNetCore.Blazor;

namespace FN18.Blazor.Client.Services
{

    public class TicketService
    {
        private readonly HttpClient _httpClient;

        public TicketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri("http://localhost:7777/api/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
            _httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Methods", "*");
            _httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Headers", "*");
            _httpClient.DefaultRequestHeaders.Add("Access-Control-Max-Age", "86400");
        }

        public async Task<ApiObjectResponse<TicketEntity>> GetTicket(string ticketId)
        {
            return await _httpClient.ApiGetAsync<TicketEntity>($"api/ticket/{ticketId}");
        }

        public async Task<ApiObjectResponse<List<TicketEntity>>> GetTickets()
        {
            return await _httpClient.ApiGetAsync<List<TicketEntity>>("api/ticket");
        }

        public async Task<ApiObjectResponse<TicketEntity>> CreateTicket(TicketEntity ticket)
        {
            return await _httpClient.ApiPostAsync<TicketEntity>("api/ticket", ticket);
        }

        //public async Task<ApiResponse> DeleteTask(string id)
        //{
        //    return await _httpClient.ApiDeleteAsync($"tasks/{id}");
        //}

        //public async Task UpdateTask(TodoTask task)
        //{
        //    await _httpClient.PutJsonAsync($"tasks/{task.Id}", task);
        //}
    }

}
