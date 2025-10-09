using System.Web.Mvc;
using StackOverflow.Services;
using StackOverflow.ViewModels;

namespace StackOverflow.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        // GET
        [Route("users/{id:int?}")]
        public ActionResult Index(int? id)
        {
            ViewBag.Id = id;
            if (id != null)
                return View();
            return View();
        }

        // GET
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl; 
            return View();
        }


        [HttpPost]
        public ActionResult Login(UserLoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.VerifyUser(model.UserName, model.Password);
            if (user != null)
            {
                // Lưu UserID và UserName vào Session
                Session["UserID"] = user.UserId;
                Session["UserName"] = user.UserName;
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
        
                return RedirectToAction("Index", "Questions");
            }

            ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu");
            return View(model);
        }
        
        // GET
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                if (_userService.Register(model))
                {
                    // Lưu thông báo thành công vào TempData
                    TempData["SuccessMessage"] = "Đăng ký tài khoản thành công!";
                    return RedirectToAction("Index", "Login");
                }
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc email đã tồn tại.");
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Questions");       
        }
    }
}