using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using HomeServiceWebApp.Models;

namespace HomeServiceWebApp.ViewModel
{
    public class UserReviewViewModel
    {
        [Key]
        public int Id { get; set; }

        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }

        [Required]
        public string Rating { get; set; }

        [Required]
        public string Review { get; set; }
    }
}