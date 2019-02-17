using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.Repro.Pages
{
    public class SubmissionsModel : BlazorComponent
    {
        [Inject]
        protected HttpClient httpClient { get; set; }

        public List<Submission> submissions;

        public ElementRef fileUpload;

        public bool Uploading { get; set; }

        protected override async Task OnInitAsync()
        {
            submissions = await httpClient.GetJsonAsync<List<Submission>>("http://localhost:7071/api/submissions/getall");

        }


        //private Task Handle(string msg)
        //{
        //    this._logger.LogInformation(msg);
        //    Uploading = false;
        //    StateHasChanged();
        //    return Task.CompletedTask;
        //}

        public Task RefreshData()
        {
            StateHasChanged();
            return Task.CompletedTask;
        }

        //protected void Cancel()
        //{
        //    Uploading = false;
        //    StateHasChanged();
        //}
    }

    public class Submission
    {

        public string Id { get; set; }


        public string Date { get; set; }


        public string UserId { get; set; }


        public string BlobUri { get; set; }


        public int ValidateScore { get; set; }


        public int OnlineScore { get; set; }


        public string Status { get; set; }


        public string TechProfile { get; set; }

        public string Type { get; set; }
    }
}