using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FN18.Shared;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace FN18.Client.Pages
{
    public class TicketModel : BlazorComponent
    {
        [Inject]
        protected HttpClient Http { get; set; }

        public bool Error { get; set; }

        protected TicketEntity ticket = new TicketEntity();

        protected List<TicketEntity> ticketEntities;

        protected ModeratorResult errorResult;

        //protected override async Task OnInitAsync()
        //{

        //}

        protected async Task CreateTicket()
        {
            errorResult = await Http.PostJsonAsync<ModeratorResult>("api/ticket", ticket);
            if (!errorResult.Flagged)
            {
                ticketEntities = new List<TicketEntity>();
                ticketEntities.Add(ticket);
            }
            else
            {
                Error = errorResult.Flagged;
            }
            StateHasChanged();
        }
    }


}