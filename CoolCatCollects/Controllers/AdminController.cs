using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Cool Cat Collects Admin Area";

            return View();
        }
    }
}