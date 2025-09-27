using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using StackOverflow.Models;
using StackOverflow.ViewModels;

namespace StackOverflow.Controllers
{
    public class LoginController : Controller
    {
        // GET
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var db = new Database();
            var conn = db.Connection();

            var command = new SqlCommand("SELECT * FROM Users WHERE username = @username AND password = @password", conn);

            command.Parameters.AddWithValue("@username", model.Username);
            command.Parameters.AddWithValue("@password", model.Password);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Close();
                Session["UserName"] = model.Username;
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