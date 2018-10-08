using System;
using System.Text.RegularExpressions;

namespace FN18.Core
{
    public class TicketEntity
    {
        public TicketEntity()
        {
            Id = Guid.NewGuid().ToString();
            Date = DateTime.UtcNow.ToShortDateString();
        }

        public string Id { get; set; }
        public string Date { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = MaskEmail(value);
            }
        }

        string _email;

        public string MaskEmail(string input)
        {
            string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
            return Regex.Replace(input, pattern, m => new string('*', m.Length));
        }
    }
}
