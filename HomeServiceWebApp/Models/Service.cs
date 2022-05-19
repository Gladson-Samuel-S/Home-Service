using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeServiceWebApp.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        public string ServiceName { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [Display(Name = "Available Time")]
        public string AvailableTime { get; set; }

        public double Price { get; set; }

        public string Location { get; set; }

        [Display(Name = "Google Maps Link")]
        public string MapUrl { get; set; }
        
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}