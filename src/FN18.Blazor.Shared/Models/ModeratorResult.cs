using System;
using System.Collections.Generic;
using System.Text;

namespace FN18.Blazor.Shared.Models
{
    public class ModeratorResult
    {
        public string Id { get; set; }
        public string OriginalText { get; set; }
        public string Email { get; set; }
        public string Term { get; set; }
    }
}
