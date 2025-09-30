using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverflow.Models;
using System.Globalization;
using StackOverflow.Services;

namespace StackOverflow.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Questions");
        }
    }
}