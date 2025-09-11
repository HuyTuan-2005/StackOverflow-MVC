using System.Web.Mvc;
using StackOverflow.Models;

namespace StackOverflow.Controllers
{
    public class AdminController : Controller
    {
        // GET
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return RedirectToAction("dashboard");
        }
        
        // GET: DashBoard
        [HttpGet]
        public ActionResult DashBoard()
        {
            return View();
        }
        
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View("Login/Index");
        }
        
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            
            var db = new Database();
            var conn = db.Connection(username, password);
            if (conn == null)
            {
                return View("Login/Index");
            }
            else
            {
                Session["username"] = username;
                return RedirectToAction("dashboard");
            }
        }
    }
}