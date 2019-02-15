using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Portal.Shared;
using System.Threading.Tasks;
using System.Net.Http;
using Blazor.FileReader;
using System;
using System.IO;
using System.Linq;

namespace Portal.Client.Pages
{
    public class SubmissionsModel : ComponentBase
    {
        [Inject]
        protected ISubmission submissionService { get; set; }

        [Inject]
        protected HttpClient httpClient { get; set; }

        [Inject]
        protected IFileReaderService fileReaderService { get; set; }

        public List<Submission> submissions;

        public ElementRef fileUpload;

        public void OnGet()
        {
            submissionService.ListSubmissions();
        }

        protected override async Task OnParametersSetAsync()
        {
            submissions = await submissionService.ListSubmissions();
        }

        public async Task UploadFile()
        {
            var files = await fileReaderService.CreateReference(fileUpload).EnumerateFilesAsync();

            string blobUrl = await submissionService.GetBlobSas();

            //TODO - fix file name format - only works with PNG
            var uri = $"https://mjsdemo.azurefd.net/uploads/{Guid.NewGuid()}.png{blobUrl}";
        
            using (MemoryStream memoryStream = await files.First().CreateMemoryStreamAsync())
            {
                var request = BlobUploader.BlobUploadRequest(memoryStream, uri);

                var req = await httpClient.SendAsync(request);

            }
        }
    }
}