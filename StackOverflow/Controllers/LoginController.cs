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
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl; 
            return View();
        }


        [HttpPost]
        public ActionResult Index(UserLoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.VerifyUser(model.UserName, model.Password);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    // Lưu UserID và UserName vào Session
                    Session["UserID"] = user.UserId;
                    Session["UserName"] = user.UserName;
                    return Redirect(returnUrl);
                }
        
                return RedirectToAction("Index", "Questions");
            }

            ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu");
            return View(model);
        }

    }
}