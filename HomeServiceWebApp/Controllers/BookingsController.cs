using HomeServiceWebApp.Models;
using HomeServiceWebApp.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeServiceWebApp.Controllers
{
    public class BookingsController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        public IEnumerable<SelectListItem> RatingList
        {
            get
            {
                return new List<SelectListItem>()
                     {
                        new SelectListItem { Text = "Excellent", Value = "Excellent" },
                        new SelectListItem { Text = "Good", Value = "Good" },
                        new SelectListItem { Text = "Poor", Value = "Poor" },
                     };
            }
            set { }
        }

        public BookingsController()
        {
            _context = new ApplicationDbContext();
        }

        public BookingsController(ApplicationUserManager userManager)
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

        [Authorize(Roles = "User, Vendor")]
        public ActionResult Index()
        {
            if (User.IsInRole("Vendor"))
            {
                return RedirectToAction("MyBookingsVendor");
            }
            else
            {
                return RedirectToAction("MyBookingsUser");
            }
        }

        [Authorize(Roles = "User")]
        public ActionResult MyBookingsUser()
        {
            var currentUserId = User.Identity.GetUserId();

            var bookingsInDb = _context.Orders.Where(u => u.ApplicationUserId == currentUserId).OrderByDescending(d => d.CreationDate).Include(s => s.Service).ToList();

            if(bookingsInDb == null || bookingsInDb.Count <= 0) 
            {
                ViewBag.Error = "No Bookings Found!";
                return View();
            }

            return View(bookingsInDb);
        }

        [Authorize(Roles = "Vendor")]
        public ActionResult MyBookingsVendor()
        {
            var currentUserId = User.Identity.GetUserId();
            var bookingsInDb = _context.Orders.Where(u => u.Service.ApplicationUserId == currentUserId).OrderByDescending(d => d.CreationDate).Include(s => s.Service).ToList();

            if (bookingsInDb == null || bookingsInDb.Count <= 0)
            {
                ViewBag.Error = "No Bookings for any services Found!";
                return View();
            }

            return View(bookingsInDb);
        }

        [Authorize(Roles = "Vendor")]
        [HttpPost]
        public ActionResult UpdateStatus(int id, string Status)
        {
            var bookingsInDb = _context.Orders.SingleOrDefault(o => o.Id == id);

            if (bookingsInDb != null && !string.IsNullOrEmpty(Status))
            {
                bookingsInDb.IsFinished = Status;
                _context.SaveChanges();
            }

            return RedirectToAction("MyBookingsVendor");
        }

        [Authorize(Roles = "User")]
        public ActionResult UserReview(int id)
        {
            ViewBag.RatingList = RatingList;
            return View();
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult UserReview(int id, string Rating, string Review)
        {
            ViewBag.RatingList = RatingList;

            if (string.IsNullOrEmpty(Rating) && string.IsNullOrEmpty(Review))
            {
                ViewBag.Error = "Please fill all the fields";
                return View();
            }
            else
            {
                var order = _context.Orders.SingleOrDefault(o => o.Id == id);

                var newUserReview = new UserReview
                {
                    Rating = Rating,
                    Review = Review
                };
                var result = _context.UserReviews.Add(newUserReview);

                order.UserReviewId = result.Id;

                _context.SaveChanges();

                return RedirectToAction("MyBookingsUser");
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