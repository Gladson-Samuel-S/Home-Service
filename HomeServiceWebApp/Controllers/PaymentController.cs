using HomeServiceWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HomeServiceWebApp.Controllers
{
    public class PaymentController : Controller
    {
        private const string _key = "rzp_test_Kr5dmBRRhAD0xX";
        private const string _secret = "WKAnFIf7X5Z47Z97yEtTNbIj";

        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;
        public PaymentController()
        {
            _context = new ApplicationDbContext();
        }

        public PaymentController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Payment(int id)
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            var currentServiceInDb = _context.Services.SingleOrDefault(s => s.Id == id);
            decimal servicePrice = 0;

            if (currentUser.IsDiscount == true)
                servicePrice = (decimal)(currentServiceInDb.Price * currentUser.Discount);
            else
                servicePrice = (decimal)(currentServiceInDb.Price);

            Random random = new Random();
            string transactionId = random.Next(10000000, 100000000).ToString();

            RazorpayClient client = new RazorpayClient(_key, _secret);

            var options = new Dictionary<string, object>();

            options.Add("amount", servicePrice * 100);
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "0");
            Order orderResponse = client.Order.Create(options);

            string orderId = orderResponse["id"].ToString();

            var razorPay = new RazorPayOptions
            {
                OrderId = orderId,
                Key = _key,
                AmountInSubUnits = servicePrice * 100,
                Currency = "INR",
                UserName = currentUser.UserName,
                UserEmail = currentUser.Email,
                UserContact = currentUser.PhoneNumber,
                UserAddress = "Chennai",
                Description = "Testing",
                ServiceId = id,
            };

            return View(razorPay);
        }

        public ActionResult BillGeneration(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return HttpNotFound();
            
            return View(order);
        }

        public ActionResult Failure()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Complete(int id)
        {
            var currentServiceInDb = _context.Services.SingleOrDefault(s => s.Id == id);
            var currentUserId = User.Identity.GetUserId();

            string paymentId = Request.Params["rzp_paymentid"];
            string orderId = Request.Params["rzp_orderid"];

            RazorpayClient client = new RazorpayClient(_key, _secret);

            Payment payment = client.Payment.Fetch(paymentId);

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            double amt = (double)paymentCaptured.Attributes["amount"];
            var creationDate = DateTime.Now;
            var price = amt / 100;

            if (paymentCaptured.Attributes["status"] == "captured")
            {
                var currentUserName = User.Identity.Name;
                var VendorId = currentServiceInDb.ApplicationUserId;
                var VendorName = $"{currentServiceInDb.ApplicationUser.FirstName} {currentServiceInDb.ApplicationUser.LastName}";

                var emailBodyForUser = $"Dear Customer {currentUserName}, Thanks for paying ₹ {price} for {currentServiceInDb.ServiceName} by {VendorName} Your OrderId = {orderId}";
                var emailBodyForVendor = $"Dear Vendor {VendorName}, New service booked for {currentServiceInDb.ServiceName} by {currentUserName}";

                await UserManager.SendEmailAsync(currentUserId, $"Payment for {currentServiceInDb.ServiceName}", emailBodyForUser);
                await UserManager.SendEmailAsync(VendorId, $"Payment for {currentServiceInDb.ServiceName}", emailBodyForVendor);

                var customerOrder = new Orders
                {
                    ApplicationUserId = currentUserId,
                    ServiceId = id,
                    Price = price,
                    OrderId = orderId,
                    TransactionId = paymentId,
                    CreationDate = creationDate,
                    IsFinished = "In Progress",
                    UserReviewId = null,
                };

                var result = _context.Orders.Add(customerOrder);
                _context.SaveChanges();
                return RedirectToAction("BillGeneration", new {id = result.Id});
            }
            else
            {
                return RedirectToAction("Failure");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}