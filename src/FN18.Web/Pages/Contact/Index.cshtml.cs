using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.RazorPages.Pages.Contact
{
    public class IndexModel : PageModel
    {
        private readonly IModeratorService _moderatorService;

        public IndexModel(IModeratorService moderatorService) => _moderatorService = moderatorService;

        [BindProperty]
        public Contact Contact { get; set; }
        public void OnGet()
        {

        }

        public void OnPost()
        {
            var result = _moderatorService.GetModeratorClient(Contact.Description);
            
            Console.WriteLine(Contact.Description);
        }
    }

    public class Contact
    {
        public string Description { get; set; }
    }
}