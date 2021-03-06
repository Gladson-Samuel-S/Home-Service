using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace HomeServiceWebApp.Models
{
    public class Orders
    {
        [Key]
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }

        public double Price { get; set; }

        public string OrderId { get; set; }

        public string TransactionId { get; set; }

        [Display(Name = "Booked On")]
        public DateTime CreationDate { get; set; }

        public IEnumerable<SelectListItem> Status
        {
            get
            {
                return new List<SelectListItem>()
                     {
                        new SelectListItem { Text = "In Progress", Value = "In Progress" },
                        new SelectListItem { Text = "Completed", Value = "Completed" },
                     };
            }
            set { }
        }

        [Display(Name = "Status")]
        public string IsFinished { get; set; }

        public int? UserReviewId { get; set; }

        [ForeignKey("UserReviewId")]
        public virtual UserReview Review { get; set; }
    }
}