using HomeServiceWebApp.Models;
using HomeServiceWebApp.ViewModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace HomeServiceWebApp.Controllers
{
    [Authorize(Roles = "Admin, User, Vendor")]
    public class MarketPlaceController : Controller
    {
        private ApplicationDbContext _context;
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

        public MarketPlaceController() { _context = new ApplicationDbContext(); }

        // GET: MarketPlace
        public ActionResult Index(int? CategoryId, string Location)
        {
            IEnumerable<Service> services = null;

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

            return View(services);
        }

        public ActionResult ServiceDetails(int id)
        {
            var service = _context.Services.FirstOrDefault(s => s.Id == id);
            return View(service);
        }
    }
}