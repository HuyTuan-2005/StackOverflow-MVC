using System.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using StackOverflow.Models;
using System.Web;
using System.Data.SqlClient;
using StackOverflow.ViewModels;
using System.Numerics;

namespace StackOverflow.Controllers
{
    public class AdminController : Controller
    {
        private List<Profile> GetAllProfile()
        {
            SqlConnection conn = new SqlConnection(Database.ConnString);
            conn.Open();
            var command = new SqlCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM profiles join users on profiles.user_id = users.user_id";
            command.Connection = conn;
            
            List<Profile> lstProfile = new List<Profile>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var profile = new Profile()
                    {
                        DisplayName = reader["display_name"].ToString(),
                        Birthday = Convert.ToDateTime(reader["Birthday"] == DBNull.Value ? DateTime.Now : reader["Birthday"]),
                        Gender = reader["Gender"] == DBNull.Value ? "Chưa xác định": reader["Gender"].ToString(),
                    };
                    lstProfile.Add(profile);
                }
            }
            return lstProfile;
        }
        
        
        // GET
        public ActionResult Index()
        {
            return RedirectToAction("dashboard");
        }
        
        // GET: DashBoard
        [HttpGet]
        public ActionResult DashBoard()
        {

            var profiles = GetAllProfile();

            // var parameters = ThongKeDashboard();

            return View("Dashboard", profiles);
        }


        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View("Login/Index", new UserLoginViewModel());
        }
        
        [HttpPost]
        public ActionResult Login(UserLoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View("Login/Index", user);
            }
            var db = new Database();

            using (var conn = db.Connection(user.UserName, user.Password))
            {
                if (conn == null)
                {
                    ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu");
                    return View("Login/Index", user);
                }
                else
                {
                    Session["admin"] = user.UserName;
                    return RedirectToAction("dashboard");
                }
            }
        }

        public ActionResult Logout()
        {
            Session["admin"] = null;
            return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public ActionResult Export()
        {
            try
            {
                BCP b = new BCP(@"localhost\SQLEXPRESS", "Forum");
                string tableName = "QUESTIONS";

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
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                {
                    return View("Dasboard");
                }
                string tempFile = Path.GetTempFileName();
                file.SaveAs(tempFile);

                BCP b = new BCP(@"localhost\SQLEXPRESS", "Forum");
                string tableName = "QUESTIONS";

                string result = b.ImportTable(tableName, tempFile);

                if (System.IO.File.Exists(tempFile))
                    System.IO.File.Delete(tempFile);

                return View("Dashboard");
            }
            catch (Exception ex)
            {
                return View("Dashboard");
            }
        }
        public SqlParameter[] ThongKeDashboard()
        {
            using (var conn = new SqlConnection(Database.ConnString))
            {
                conn.Open();

                // cmdText = tên stored procedure
                using (var command = new SqlCommand("sp_ThongKeDashboard", conn))
                {
                    // CommandType = StoredProcedure sử dụng stored procedure trong SQL Server
                    command.CommandType = CommandType.StoredProcedure;

                    // khai báo giá trị output trong sql server
                    SqlParameter[] parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@COUNT_USERS", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    parameters[1] = new SqlParameter("@COUNT_QUESTIONS", SqlDbType.Int) { Direction = ParameterDirection.Output }; 
                    parameters[2] = new SqlParameter("@COUNT_ANSWERS", SqlDbType.Int) { Direction = ParameterDirection.Output }; 
                    parameters[3] = new SqlParameter("@COUNT_TAGS", SqlDbType.Int) { Direction = ParameterDirection.Output }; 
                    // Direction = Output
                    foreach (var p in parameters)
                    {
                        command.Parameters.Add(p);
                    }
                    command.ExecuteNonQuery();

                    return parameters;
                }
            }
        }
        
        public ActionResult Privilege()
        {
            return View();
        }
    }
}