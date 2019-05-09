using FN18.Core;
using Microsoft.AspNetCore.Blazor.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FN18.Blazor.Pages
{
    public class TicketModel : BlazorComponent
    {
        [Inject]
        protected HttpClient Http { get; set; }

        public bool Error { get; set; }

        protected TicketEntity ticket = new TicketEntity();

        protected List<TicketEntity> ticketEntities;

        public ModeratorResult errorResult { get; set; }

        protected async Task CreateTicket()
        {
            errorResult = await HttpClientExtension.PostJsonCustomAsync<ModeratorResult>(Http, "https://mjsdemoblazorfunc.azurewebsites.net/api/Moderator", ticket);
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

        protected void OnErrorAlertClosed()
        {
            Error = false;
            StateHasChanged();
        }
    }


}