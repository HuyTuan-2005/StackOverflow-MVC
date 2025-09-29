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

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        
        // GET
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _userService.VerifyUser(model);
            if (result == 1)
            {
                return RedirectToAction("Index", "Questions");
            }

            ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu");
            return View(model);
        }
    }
}