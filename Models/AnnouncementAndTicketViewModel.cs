using System.ComponentModel.DataAnnotations;
namespace TicketApplication.Models
{
    public class AnnouncementAndTicketViewModel
    {
        public string? Title { get; set; }

        public List<Announcements>? Announcements { get; set; }
        public List<Ticket>? Ticket { get; set; }
    }
}
