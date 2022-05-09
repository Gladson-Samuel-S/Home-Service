using System.Web.Mvc;

namespace HomeServiceWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]

        public ActionResult About()
        {
            return View();
        }
    }
}