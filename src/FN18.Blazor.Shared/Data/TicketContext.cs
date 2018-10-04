using Microsoft.EntityFrameworkCore;

namespace FN18.Blazor.Shared.Data
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options) { }

        public DbSet<TicketEntity> TicketEntities { get; set; }
    }
}
