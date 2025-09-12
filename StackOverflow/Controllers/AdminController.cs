using System.Web.Mvc;
using StackOverflow.Models;

namespace StackOverflow.Controllers
{
    public class AdminController : Controller
    {
        // GET
        [LoginAuthenticationFilter]
        public ActionResult Index()
        {
            return RedirectToAction("dashboard");
        }
        
        // GET: DashBoard
        [HttpGet]
        [LoginAuthenticationFilter]
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
        public ActionResult Login(User user)
        {
            
            var db = new Database();
            var conn = db.Connection(user.UserName, user.Password);
            if (conn == null)
            {
                return View("Login/Index");
            }
            else
            {
                Session["admin"] = user.UserName;
                return RedirectToAction("dashboard");
            }
        }

        public ActionResult Logout()
        {
            Session["admin"] = null;
            return RedirectToAction("Login", "Admin");
        }
    }
}