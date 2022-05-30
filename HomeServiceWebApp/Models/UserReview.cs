using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeServiceWebApp.Models
{
    public class UserReview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Rating { get; set; }

        [Required]
        public string Review { get; set; }
    }
}