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

        public string OrderId { get; set; }

        public string TransactionId { get; set; }

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

        public string IsFinished { get; set; }
    }
}