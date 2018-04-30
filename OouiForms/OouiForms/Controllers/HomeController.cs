using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OouiForms.Models;
using OouiForms.Pages;

using Ooui.AspNetCore;
using Xamarin.Forms;

namespace OouiForms.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var page = new ShopPage();
            var element = page.GetOouiElement();
            return new ElementResult(element);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
