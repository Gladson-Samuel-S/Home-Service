using HomeServiceWebApp.Models;
using HomeServiceWebApp.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HomeServiceWebApp.Controllers
{
    [Authorize(Roles = "Admin, User, Vendor")]
    public class MarketPlaceController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;

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

        public MarketPlaceController()
        {
            _context = new ApplicationDbContext();
        }

        public MarketPlaceController(ApplicationUserManager userManager) 
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

        public async Task CheckDiscount()
        {
            List<string> userIds = new List<string>();

            var userOrders = _context.Orders.ToList();
            var currentUserId = User.Identity.GetUserId();
            var currentUser = UserManager.FindById(currentUserId);
            var maxServiceOrderedByUser = userOrders.GroupBy(n => n.ApplicationUserId)
                                 .Select(n => new
                                 {
                                     UserName = n.Select(u => u.ApplicationUser.FirstName),
                                     UserId = n.Select(u => u.ApplicationUserId),
                                     UserCount = n.Count()
                                 })
                                 .OrderByDescending(n => n.UserCount).Take(5).ToList();

            foreach (var item in maxServiceOrderedByUser)
            {
                userIds.AddRange(item.UserId);
            }

            if (userIds.Contains(currentUserId))
            {
                currentUser.IsDiscount = true;
                currentUser.Discount = 0.8;
                await UserManager.UpdateAsync(currentUser);
            }
        }

        // GET: MarketPlace
        public async Task<ActionResult> Index(int? page, int? CategoryId, string Location)
        {
            IEnumerable<Service> services = null;
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            await CheckDiscount();
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.CurrentUser = currentUser;

            var categories = _context.Category.ToList();
            ViewBag.Categories = categories;
            ViewBag.City = City;

            if(!string.IsNullOrEmpty(Location) && CategoryId != null)
            {
                services = _context.Services
                                .Include(c => c.Category)
                                .Where(c => c.CategoryId == CategoryId)
                                .Where(l => l.Location == Location)
                                .ToList();

                var selectedCategory = _context.Category.SingleOrDefault(Category => Category.Id == CategoryId);
                ViewBag.SelectedCategory = selectedCategory.Name;
                ViewBag.SelectedLocation = Location;
            }
            else if (CategoryId != null)
            {
                services = _context.Services
                                .Where(c => c.CategoryId == CategoryId)
                                .Include(c => c.Category)
                                .ToList();

                var selectedCategory = _context.Category.SingleOrDefault(Category => Category.Id == CategoryId);
                ViewBag.SelectedCategory = selectedCategory.Name;
            }
            else if (!string.IsNullOrEmpty(Location))
            {
                services = _context.Services
                                .Where(l => l.Location == Location)
                                .Include(c => c.Category)
                                .ToList();

                ViewBag.SelectedLocation = Location;
            }
            else services = _context.Services.Include(c => c.Category).ToList();


            if (!services.Any()) 
            {
                ViewBag.SelectedCategory = null;
                ViewBag.SelectedLocation = null;
                ViewBag.NotFound = "No Services Found 😔"; 
            }

            return View(services.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ServiceDetails(int id)
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.CurrentUser = currentUser;
            var service = _context.Services.FirstOrDefault(s => s.Id == id);
            var userReviewsForService = _context.Orders.Where(o => o.ServiceId == id && o.UserReviewId != null).OrderByDescending(o => o.CreationDate).Include(r => r.Review).Take(5).ToList();

            if (userReviewsForService.Any() && userReviewsForService.Count() > 0)   
                ViewBag.UserReviews = userReviewsForService;
            else 
                ViewBag.UserReviews = null; 
            return View(service);
        }
        
        public ActionResult ViewVendorDetails(string vendorId)
        {
            var vendorInDb = UserManager.FindById(vendorId);
            var vendorInDbDetails = new ViewVendorDetailsViewModel
            {
                FirstName = vendorInDb.FirstName,
                LastName = vendorInDb.LastName,
                Age = vendorInDb.Age,
                PhoneNumber = vendorInDb.PhoneNumber,
                Email = vendorInDb.Email,
                Address = vendorInDb.Vendor != null ? vendorInDb.Vendor.Address : "No Address Added",

            };
            ViewBag.VendorDetails = vendorInDbDetails;

            return View(vendorInDbDetails);
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