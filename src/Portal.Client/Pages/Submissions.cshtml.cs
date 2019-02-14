using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Portal.Shared;
using System.Threading.Tasks;

namespace Portal.Client.Pages
{
    public class SubmissionsModel : ComponentBase
    {
        [Inject]
        protected ISubmission submissionService { get; set; }

        public List<Submission> submissions;
        public void OnGet()
        {
            submissionService.ListSubmissions();
        }

        protected override async Task OnParametersSetAsync()
        {
            submissions = await submissionService.ListSubmissions();
        }
    }
}