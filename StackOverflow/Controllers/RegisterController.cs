using System.Web.Mvc;
using StackOverflow.Services;
using StackOverflow.ViewModels;

namespace StackOverflow.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserService _userServices;
        public RegisterController(IUserService userService)
        {
            _userServices = userService;
        }
        // GET
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                if (_userServices.Register(model))
                {
                    // Lưu thông báo thành công vào TempData
                    TempData["SuccessMessage"] = "Đăng ký tài khoản thành công!";
                    return RedirectToAction("Index", "Login");
                }
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc email đã tồn tại.");
                return View(model);
            }
        }
    }
}