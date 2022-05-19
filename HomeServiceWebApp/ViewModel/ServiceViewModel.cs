using HomeServiceWebApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace HomeServiceWebApp.ViewModel
{
    public class AddServiceViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please Select a Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual IEnumerable<Category> Category { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Phone number is not valid")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public IEnumerable<SelectListItem> Time
        {
            get
            {
                return new List<SelectListItem>()
                     {
                        new SelectListItem { Text = "6am - 10am", Value = "6am - 10am" },
                        new SelectListItem { Text = "9am - 12pm", Value = "9am - 12pm" },
                        new SelectListItem { Text = "12pm - 6pm", Value = "12pm - 6pm" },
                     };
            }
            set { }
        }

        [Display(Name = "Available Time")]
        [Required(ErrorMessage = "Please select an available time")]
        public string AvailableTime { get; set; }

        [Required(ErrorMessage = "Please enter a price for the service")]
        [Range(100, 100000, ErrorMessage = "Please enter a price within this range")]
        public double Price { get; set; }

        public IEnumerable<SelectListItem> City
        {
            get
            {
                return new List<SelectListItem>()
                     {
                        new SelectListItem { Text = "Chennai", Value = "Chennai" },
                        new SelectListItem { Text = "Coimbatore", Value = "Coimbatore" },
                        new SelectListItem { Text = "Delhi", Value = "Delhi" },
                        new SelectListItem { Text = "Mumbai", Value = "Mumbai" },
                     };
            }
            set { }
        }

        [Required(ErrorMessage = "Please select a city")]
        public string Location { get; set; }

        [Display(Name = "Map Link")]
        [Required]
        public string MapUrl { get; set; }
    }
}