using System.ComponentModel.DataAnnotations;

namespace TicketApplication.Models
{
    public class Announcements
    {
        public int Id { get; set; }

        [Required, Display(Name ="Announcement Title")]
        public string Title { get; set; }
        [Required, Display(Name ="Announcement Body")]
        public string Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required, Display(Name ="Author")]
        public string OwnerName { get; set; } 
    }
}
