using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverflow.Models;

namespace StackOverflow.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            // if (Session["username"] == null)
            // {
            //     return RedirectToAction("Index", "Login");
            // }
            return View();
        }
    }
}
