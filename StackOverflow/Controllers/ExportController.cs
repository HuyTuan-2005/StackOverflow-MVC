using System;
using System.Collections.Generic;
using System.IO;
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

                string tempFile = Path.GetTempFileName();

                string result = b.ExportTable(tableName, tempFile);

                byte[] fileBytes = System.IO.File.ReadAllBytes(tempFile);

                if (System.IO.File.Exists(tempFile))
                    System.IO.File.Delete(tempFile);

                string outputFileName = $"{tableName}_export.csv";
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