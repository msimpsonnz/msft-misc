using FN18.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CognitiveServices.ContentModerator.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FN18.Web.Pages
{
    public class SubmitModel : PageModel
    {
        [TempData]
        public string text { get; set; }

        [BindProperty]
        public FormFill FormFill { get; set; }


        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            text = FormFill.Description;
            return RedirectToPage("Screen", text);
        }
    }

    public class FormFill
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Description { get; set; }
    }
}