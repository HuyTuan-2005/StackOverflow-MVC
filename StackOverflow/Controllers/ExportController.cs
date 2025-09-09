using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverflow.Models;
namespace StackOverflow.Controllers
{
    public class ExportController : Controller
    {
        // GET: NhapXuatFile
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Export()
        {
            try
            {
                BCP b = new BCP(@"localhost\SQLEXPRESS", "Forum");
                string tableName = "USERS";
                string filePath = Server.MapPath("~/export.csv");
                string result = b.ExportTable(tableName, filePath);

                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                string outputFileName = $"{tableName}" + "_export.csv";
                return File(fileBytes, "text/csv", outputFileName);
                
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index");
            }
        }
    }
}