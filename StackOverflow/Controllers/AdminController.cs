using System.Web.Mvc;
using StackOverflow.Models;

namespace StackOverflow.Controllers
{
    public class AdminController : Controller
    {
        // GET
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View("Login/Index");
        }
        
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            
            var db = new ConnectAdmin();
            var conn = db.Connection(username, password);
            if (conn == null)
            {
                return View("Login/Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}