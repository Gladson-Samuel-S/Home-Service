using System.Web.Mvc;

namespace HomeServiceWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("User"))
                return RedirectToAction("Index", "MarketPlace");
            else if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Vendor");

            return View();
        }
    }
}