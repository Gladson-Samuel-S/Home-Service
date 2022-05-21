using HomeServiceWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyBookingsUser()
        {
            var bookingsInDb = _context.Orders.Where(u => u.ApplicationUserId == User.Identity.GetUserId()).Include(s => s.Service).ToList();

            if(bookingsInDb == null) 
            {
                ViewBag.Error = "No Bookings Found";
                return View();
            }

            return View(bookingsInDb);
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