using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeServiceWebApp.Models
{
    public class RazorPayOptions
    {
        public string OrderId { get; set; }
        public string Key { get; set; }
        public decimal AmountInSubUnits { get; set; }
        public string Currency { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserContact { get; set; }
        public string UserAddress { get; set; }
        public string Description { get; set; }
        public int ServiceId { get; set; }
    }
}