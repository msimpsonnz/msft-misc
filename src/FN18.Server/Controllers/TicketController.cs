using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FN18.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FN18.Server.Controllers
{
    [Route("api/[controller]")]
    public class TicketController : Controller
    {
        private readonly IModeratorService _moderatorService;

        public TicketController(IModeratorService moderatorService)
        {
            _moderatorService = moderatorService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] TicketEntity ticketEntity)
        {
            var result = await _moderatorService.GetModeratorClient(ticketEntity.Description);
            return Ok(result);
        }
    }
}