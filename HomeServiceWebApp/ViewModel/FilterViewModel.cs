using HomeServiceWebApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace HomeServiceWebApp.ViewModel
{
    public class FilterViewModel
    {

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please Select a Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual IEnumerable<Category> Category { get; set; }

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
    }
}