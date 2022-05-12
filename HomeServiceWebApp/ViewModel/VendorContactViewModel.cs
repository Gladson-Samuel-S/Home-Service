using HomeServiceWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace HomeServiceWebApp.ViewModel
{
    public class ViewVendorDetailsViewModel
    {
        [Required(ErrorMessage = "Please Enter First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Range(18, 100)]
        public int? Age { get; set; }

        public Gender? Gender { get; set; }

        [Required]
        [Phone(ErrorMessage = "Phone number is invalid")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
    }

    public class VendorContactViewModel
    {
        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
    }

    public class EditVendorDetailsViewModel
    {
        [Required]
        [Phone(ErrorMessage = "Phone number is invalid")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
    }
}