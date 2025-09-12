using System.IO;
using System;
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

        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult Export()
        {
            try
            {
                BCP b = new BCP(@"localhost\SQLEXPRESS", "Forum");
                string tableName = "QUESTIONS";

                string tempFile = Path.GetTempFileName();

                string result = b.ExportTable(tableName, tempFile);

                byte[] fileBytes = System.IO.File.ReadAllBytes(tempFile);

                if (System.IO.File.Exists(tempFile))
                    System.IO.File.Delete(tempFile);

                string outputFileName = $"{tableName}_export.csv";
                return File(fileBytes, "text/csv", outputFileName);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index");
            }
        }
    }
}