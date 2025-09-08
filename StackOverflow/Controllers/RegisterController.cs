using System.Web.Mvc;

namespace StackOverflow.Controllers
{
    public class RegisterController : Controller
    {
        // GET
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register()
        {
            return View("Index");
        }
    }
}