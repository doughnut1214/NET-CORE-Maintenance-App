using System.ComponentModel.DataAnnotations;

namespace TicketApplication.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }

        public string? OwnerID { get; set; }
        [Required]
        [Display(Name = "Title of Issue")]
        public string Title { get; set; }   

        [Required]
        [Display(Name = "Brief Description of Issue")]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        //public bool? IsSolved { get; set; } = false;
        
        public List<TicketResponse>? TicketResponses { get; set; }
        


       

    }
}
