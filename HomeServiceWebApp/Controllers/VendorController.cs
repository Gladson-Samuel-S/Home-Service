using HomeServiceWebApp.Models;
using HomeServiceWebApp.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HomeServiceWebApp.Controllers
{
    [Authorize(Roles = "Admin, Vendor")]
    public class VendorController : Controller
    {

        private ApplicationUserManager _userManager;

        public VendorController() { }

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
                Address = currentUser.Vendor.Address,

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
            var currentUserDetails = new EditVendorDetailsViewModel() { Address = currentUser.Vendor.Address, PhoneNumber = currentUser.PhoneNumber };
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
    }
}