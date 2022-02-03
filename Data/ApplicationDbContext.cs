using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketApplication.Models;

namespace TicketApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TicketApplication.Models.Ticket> Ticket { get; set; }
        public DbSet<TicketApplication.Models.TicketResponse> TicketResponse { get; set; }
        public DbSet<TicketApplication.Models.Announcements> Announcements { get; set; }
        
    }
}