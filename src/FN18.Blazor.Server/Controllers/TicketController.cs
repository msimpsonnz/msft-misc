using FN18.Blazor.Shared;
using FN18.Blazor.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FN18.Blazor.Server.Controllers
{
    [Route("api/ticket")]
    public class TicketController : Controller
    {
        private readonly TicketContext _context;

        public TicketController(TicketContext context)
        {
            _context = context;

        }

        [HttpGet]
        public IEnumerable<TicketEntity> GetTickets()
        {
            return _context.TicketEntities;
        }


        // POST: api/ToDo
        [HttpPost]
        public async Task<IActionResult> PostContact([FromBody] TicketEntity ticketEntity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _context.TicketEntities.Add(ticketEntity);
            await _context.SaveChangesAsync();
            return CreatedAtAction("ticket/fetch", ticketEntity.Id);
        }
    }
}
