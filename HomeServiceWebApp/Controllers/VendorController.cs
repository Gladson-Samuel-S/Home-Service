using HomeServiceWebApp.Models;
using HomeServiceWebApp.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HomeServiceWebApp.Controllers
{
    [Authorize(Roles = "Admin, Vendor")]
    public class VendorController : Controller
    {

        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;

        public VendorController() { _context = new ApplicationDbContext(); }

        public VendorController(ApplicationUserManager userManager)
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

        // GET: Vendor
        [HttpGet]
        public ActionResult Index()
        {
            var showAddContactsLink = true;
            var currentUser = UserManager.FindById(User.Identity.GetUserId());

            if (currentUser.VendorId != null) showAddContactsLink = false;
            ViewBag.ShowAddContactsLink = showAddContactsLink;
            ViewBag.Message = TempData["Message"];
            return View();
        }

        [HttpGet]
        public ActionResult ViewVendorDetails()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            var currentUserDetails = new ViewVendorDetailsViewModel
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Age = currentUser.Age,
                PhoneNumber = currentUser.PhoneNumber,
                Email = currentUser.Email,
                Address = currentUser.Vendor != null ? currentUser.Vendor.Address : "No Address Added",

            };
            ViewBag.VendorDetails = currentUserDetails;

            return View(currentUserDetails);
        }

        [HttpGet]
        public ActionResult AddContactDetails()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddContactDetails(VendorContactViewModel vendor)
        {
            if(!ModelState.IsValid)
                return View(vendor);

            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            var vendorAddress = new VendorContact { Address = vendor.Address };

            if(currentUser != null)
                currentUser.Vendor = vendorAddress;    

            var result = await UserManager.UpdateAsync(currentUser);

            if (result.Succeeded)
                TempData["Message"] = $"Successfully Added Contact Information";
            else
                TempData["Message"] = $"Failed to add contact information :(";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditVendorDetails()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            var currentUserDetails = new EditVendorDetailsViewModel() { Address = currentUser.Vendor != null ? currentUser.Vendor.Address : "No Address Added", PhoneNumber = currentUser.PhoneNumber };
            return View(currentUserDetails);
        }

        [HttpPost]
        public async Task<ActionResult> EditVendorDetails(EditVendorDetailsViewModel vendor)
        {
            if (!ModelState.IsValid)
                return View(vendor);

            var currentUser = UserManager.FindById(User.Identity.GetUserId());

            if (currentUser != null)
            {
                if (currentUser.Vendor == null)
                {
                    var vendorAddress = new VendorContact { Address = vendor.Address };
                    currentUser.Vendor = vendorAddress;
                }

                currentUser.Vendor.Address = vendor.Address;
                currentUser.PhoneNumber = vendor.PhoneNumber;
            }

            var result = await UserManager.UpdateAsync(currentUser);

            if (result.Succeeded)
                TempData["Message"] = $"Successfully Edited Contact Information";
            else
                TempData["Message"] = $"Failed to Edited contact information :(";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddNewService()
        {
            var categories = _context.Category.ToList();
            var viewModel = new AddServiceViewModel
            {
                Category = categories
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddNewService(AddServiceViewModel service)
        {
            var categories = _context.Category.ToList();
            var viewModel = new AddServiceViewModel
            {
                Category = categories
            };

            if (!ModelState.IsValid) 
                return View(viewModel);

            var newService = new Service
            {
                ServiceName = service.ServiceName,
                CategoryId = service.CategoryId,
                PhoneNumber = service.PhoneNumber,
                Address = service.Address,
                AvailableTime = service.AvailableTime,
                Price = service.Price,
                Location = service.Location,
                MapUrl = service.MapUrl,
                ApplicationUserId = User.Identity.GetUserId(),
            };

            _context.Services.Add(newService);
            _context.SaveChanges();

            return RedirectToAction("MyServices");
        }

        [HttpGet]
        public ActionResult MyServices()
        {
            string userId = User.Identity.GetUserId();
            var services = _context.Services.Where(s => s.ApplicationUserId == userId).ToList();
            if (services.Count == 0)
                ViewBag.Message = "No Services found";

            return View(services);
        }

        public ActionResult Delete(int id)
        {
            var serviceInDb = _context.Services.FirstOrDefault(s => s.Id == id);

            if (serviceInDb == null) return HttpNotFound();

            _context.Services.Remove(serviceInDb);
            _context.SaveChanges();
            return RedirectToAction("MyServices");
        }

        public ActionResult Edit(int id)
        {
            var categories = _context.Category.ToList();
            var serviceInDb = _context.Services.FirstOrDefault(s => s.Id == id);

            var viewModel = new AddServiceViewModel
            {
                ServiceName = serviceInDb.ServiceName,
                CategoryId = serviceInDb.CategoryId,
                PhoneNumber = serviceInDb.PhoneNumber,
                Address = serviceInDb.Address,
                AvailableTime = serviceInDb.AvailableTime,
                Price = serviceInDb.Price,
                Location = serviceInDb.Location,
                MapUrl = serviceInDb.MapUrl,
                Category = categories
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(AddServiceViewModel service)
        {
            var categories = _context.Category.ToList();
            var viewModel = new AddServiceViewModel
            {
                Category = categories
            };

            if (!ModelState.IsValid)
                return View(viewModel);

            var serviceInDb = _context.Services.FirstOrDefault(s => s.Id == service.Id);

            serviceInDb.ServiceName = service.ServiceName;
            serviceInDb.CategoryId = service.CategoryId;
            serviceInDb.PhoneNumber = service.PhoneNumber;
            serviceInDb.Address = service.Address;
            serviceInDb.AvailableTime = service.AvailableTime;
            serviceInDb.Price = service.Price;
            serviceInDb.Location = service.Location;
            serviceInDb.MapUrl = service.MapUrl;
            serviceInDb.ApplicationUserId = User.Identity.GetUserId();
            _context.SaveChanges();

            return RedirectToAction("MyServices");
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