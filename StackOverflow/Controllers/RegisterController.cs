using System.Web.Mvc;
using StackOverflow.ViewModels;

namespace StackOverflow.Controllers
{
    public class RegisterController : Controller
    {
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
            // else
            // {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc email đã tồn tại.");
                return View(model);
            // }
            return RedirectToAction("Index", "Login");
        }
    }
}