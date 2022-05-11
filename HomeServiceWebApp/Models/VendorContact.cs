using System.ComponentModel.DataAnnotations;

namespace HomeServiceWebApp.Models
{
    public class VendorContact
    {
        [Key]
        public int Id { get; set; }

        public string Address { get; set; }

    }
}