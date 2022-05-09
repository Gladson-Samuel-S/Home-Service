using System.Web.Mvc;

namespace HomeServiceWebApp.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
    }
}