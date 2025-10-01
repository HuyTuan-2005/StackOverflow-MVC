using System.Web.Mvc;

namespace StackOverflow.Controllers
{
    public class UsersController : Controller
    {
        // GET
        public ActionResult Index(int? id)
        {
            ViewBag.Id = id;
            if (id != null)
                return View();
            return View();
        }
    }
}