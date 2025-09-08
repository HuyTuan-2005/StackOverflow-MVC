using System;
using System.Web.Mvc;
using StackOverflow.Models;

namespace StackOverflow.Controllers
{
    public class LoginController : Controller
    {
        // GET
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login()
        { 
            var db = new Database();
            var conn = db.Connection();

            var username = Request.Form.Get("username");
            var password = Request.Form.Get("password");

            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM Users WHERE username = @username AND password = @password";
            
            command.CommandText = "SELECT * FROM Users WHERE username = @username AND password = @password";
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            
            
            var reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {
                reader.Close();
                Session["UserName"] = username;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                reader.Close();
                return View("Index");
            }
        }
    }
}