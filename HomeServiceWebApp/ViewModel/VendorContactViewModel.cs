using HomeServiceWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace HomeServiceWebApp.ViewModel
{
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