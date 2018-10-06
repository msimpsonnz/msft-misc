using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FN18.Blazor.Shared;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;

namespace FN18.Blazor.Client.Pages
{
    public class TicketDataModel : BlazorComponent
    {
        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        protected IUriHelper UriHelper { get; set; }

        [Parameter]
        protected string paramTicketId { get; set; } = "0";
        [Parameter]
        protected string action { get; set; }

        public bool Error { get; set; }

        protected List<TicketEntity> ticketList = new List<TicketEntity>();
        protected TicketEntity ticket = new TicketEntity();
        protected string title { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (action == "fetch")
            {
                await FetchTickets();
                this.StateHasChanged();
            }
            else if (action == "create")
            {
                title = "Create Ticket";
                ticket = new TicketEntity();
            }
            else if (paramTicketId != "0")
            {
                if (action == "edit")
                {
                    title = "Edit Ticket";
                }
                else if (action == "delete")
                {
                    title = "Delete Ticket";
                }

                ticket = await Http.GetJsonAsync<TicketEntity>("/api/ticket" + Convert.ToInt32(paramTicketId));
            }
        }

        protected async Task FetchTickets()
        {
            title = "Ticket Data";
            ticketList = await Http.GetJsonAsync<List<TicketEntity>>("api/ticket");
        }

        protected async Task CreateTicket()
        {
            //if (ticket.Id != "0")
            //{
            //    await Http.SendJsonAsync(HttpMethod.Put, "api/ticket", ticket);
            //}
            //else
            //{
            //    await Http.SendJsonAsync(HttpMethod.Post, "/api/ticket", ticket);
            //}
            var result = await Http.SendJsonAsync<TicketEntity>(HttpMethod.Post, "api/ticket", ticket);
            if (result.Id != null)
            {
                Error = true;
            }
            StateHasChanged();
        }

        protected async Task DeleteTicket()
        {
            await Http.DeleteAsync("api/ticket" + Convert.ToInt32(paramTicketId));
            UriHelper.NavigateTo("ticket/fetch");
        }

        protected void Cancel()
        {
            title = "Ticket Data";
            UriHelper.NavigateTo("ticket/fetch");
        }
    }
}
