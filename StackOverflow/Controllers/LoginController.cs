using System;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Mvc;
using StackOverflow.Models;
using StackOverflow.Services;
using StackOverflow.ViewModels;

namespace StackOverflow.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
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

            return View(model);
        }
    }
}