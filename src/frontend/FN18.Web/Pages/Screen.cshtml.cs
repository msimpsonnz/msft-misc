using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FN18.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CognitiveServices.ContentModerator.Models;
using Microsoft.Extensions.Configuration;

namespace FN18.Web.Pages
{
    public class ScreenModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public ScreenModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [TempData]
        public string text { get; set; }
        [BindProperty]
        public Screen ScreenResult { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var client = ModeratorHelper.contentModeratorClient(_configuration["ContentModeratorRegion"], _configuration["ContentModeratorKey"]);
            ScreenResult = await ModeratorHelper.TextScreen(client, text);
            return Page();
        }
    }
}