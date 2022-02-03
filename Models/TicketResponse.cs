using System.ComponentModel.DataAnnotations;

namespace TicketApplication.Models
{
    public class TicketResponse
    {
        public int Id { get; set; }
        
        [Required, Display(Name = "Response Title")]
        public string Title { get; set; }
        [Required, Display(Name ="Response")]
        public string Description { get; set; }

        [Display(Name ="Date of Response")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public int TicketId { get; set; }

        /* 
         TO TRY: kill Ticket ticket and kill the list of responses in ticket model 
         */
        
        //public Ticket Ticket { get; set; }


    }
}
