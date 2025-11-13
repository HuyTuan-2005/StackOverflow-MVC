using System.Web.Mvc;

namespace StackOverflow.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class PrivilegeController : Controller
    {
        // GET: Admin/Privilege
        public ActionResult Index()
        {
            return View();
        }
    }
}