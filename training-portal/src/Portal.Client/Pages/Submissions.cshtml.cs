using Blazor.Extensions;
using Blazor.FileReader;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.Extensions.Logging;
using Portal.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Portal.Client.Pages
{
    public class SubmissionsModel : BlazorComponent
    {
        [Inject]
        protected ISubmission submissionService { get; set; }

        [Inject]
        protected HttpClient httpClient { get; set; }

        [Inject]
        protected IFileReaderService fileReaderService { get; set; }

        [Inject] private ILogger<SubmissionsModel> _logger { get; set; }

        public List<Submission> submissions;

        public ElementRef fileUpload;

        private HubConnection connection;

        public IEnumerable<IFileReference> files;

        public bool Uploading { get; set; }

        protected override async Task OnInitAsync()
        {
            submissions = await submissionService.ListSubmissions();
            //string signalr = "http://localhost:7071/api/SignalRInfo";
            string signalr = "https://mjsdemo.azurefd.net/api/SignalRInfo";

            var signalrInfo = await httpClient.GetJsonAsync<SignalRInfo>(signalr);


            connection = new HubConnectionBuilder()
                .WithUrl(signalrInfo.url,
                    opt =>
                    {
                        opt.Transport = HttpTransportType.WebSockets;
                        opt.AccessTokenProvider = async () =>
                        {
                            var token = await httpClient.GetJsonAsync<SignalRInfo>(signalr);
                            return token.accessToken;
                        };
                    })
                .Build();

            connection.On<string>("submissionUpdated", this.Handle);
            await connection.StartAsync();

        }

        public async Task SetFileRef()
        {
            files = await fileReaderService.CreateReference(fileUpload).EnumerateFilesAsync();
        }

        public async Task UploadFile()
        {
            //var files = await fileReaderService.CreateReference(fileUpload).EnumerateFilesAsync();
            Uploading = true;
            StateHasChanged();
            string blobUrl = await submissionService.GetBlobSas();
            string guid = Guid.NewGuid().ToString();

            //TODO - fix file name format - only works with PNG
            var uri = $"https://mjsdemo.azurefd.net/uploads/{guid}.png{blobUrl}";

            using (MemoryStream memoryStream = await files.First().CreateMemoryStreamAsync())
            {
                var request = BlobUploader.BlobUploadRequest(memoryStream, uri);

                var req = await httpClient.SendAsync(request);

            }
            files = null;
        }


        private async Task Handle(string msg)
        {
            this._logger.LogInformation(msg);
            Uploading = false;
            submissions = null; // this is not necessary; with this line after every click you will see
                                // that your page is rendered once with "loading" message, and then again
                                // automatically with your data - you don't have to call StateHasChanged
                                // without this line application will be nicer for the user (no screen flashing)
            submissions = await submissionService.ListSubmissions();
            StateHasChanged();
        }

        public Task Refresh()
        {
            StateHasChanged();
            return Task.CompletedTask;
        }

        protected void Cancel()
        {
            Uploading = false;
            StateHasChanged();
        }
    }
}