using FN18.Blazor.Shared;
using FN18.Blazor.Shared.Entities;
using FN18.Blazor.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FN18.Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        IModeratorService _moderatorService;
        public SampleDataController(IModeratorService moderatorService)
        {
            _moderatorService = moderatorService;
        }
  

        [HttpGet("[action]")]
        public async Task<ModeratorResult> ScreenText()
        {
            ModeratorResult screen = await _moderatorService.TextScreen("this is shit");
            return screen;
        }
    }
}
