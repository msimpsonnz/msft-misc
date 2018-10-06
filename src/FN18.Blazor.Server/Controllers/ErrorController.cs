using FN18.Blazor.Shared.Data;
using FN18.Blazor.Shared.Interfaces;
using FN18.Blazor.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.ContentModerator.Models;
using System.Collections.Generic;
using System.Linq;

namespace FN18.Blazor.Server.Controllers
{
    [Route("api/error")]
    public class ErrorController : Controller
    {
        private List<Screen> _moderatorResult;
        private readonly IModeratorService _moderatorService;

        public ErrorController(TicketContext ticketContext, List<Screen> moderatorResult, IModeratorService moderatorService)
        {
            _moderatorResult = moderatorResult;
            _moderatorService = moderatorService;
        }

        [HttpGet("{errorId}")]
        public ModeratorResult GetError(string errorId)
        {
            var rawResult = _moderatorResult.FirstOrDefault(r => r.TrackingId == errorId);
            var result = new ModeratorResult()
            {
                Id = rawResult.TrackingId,
                OriginalText = rawResult.OriginalText,
                //Email = rawResult.PII.Email[0].ToString() ?? null,
                Email = string.Empty,
                Term = rawResult.Terms[0].Term ?? null
            };
            return result;
        }
    }
}