using Microsoft.AspNetCore.Blazor.Components;
using FN18.Core;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Blazor;
using System.Collections.Generic;

namespace FN18.Blazor.Pages
{
    public class IndexModel : BlazorComponent
    {
        [Inject]
        protected HttpClient Http { get; set; }

        public List<CatalogItem> catalogItems = new List<CatalogItem>();

        protected override async Task OnParametersSetAsync()
        {
            catalogItems = await Http.GetJsonAsync<List<CatalogItem>>("data/catalog.json");
        }
    }
}