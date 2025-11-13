using StackOverflow.Helpers;
using StackOverflow.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverflow.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Account
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session["UserConnStr"] = null;
            return RedirectToAction("Login", "Admin");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            string baseConn = ConfigurationManager.ConnectionStrings["AdminForumDB"].ConnectionString;
            var builder = new SqlConnectionStringBuilder(baseConn)
            {
                UserID = user.UserName,
                Password = user.Password,
                IntegratedSecurity = false // đảm bảo không xung đột với Trusted_Connection
            };

            try
            {
                using (var conn = new SqlConnection(builder.ConnectionString))
                {
                    conn.Open();
                }
                Session["UserConnStr"] = builder.ConnectionString; // lưu lại cho phiên làm việc
                return RedirectToAction("Index", "dashboard");
            }
            catch
            {
                ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu");
                return View(user);
            }
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult Register(UserRegisterAdminViewModel model)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AdminForumDB"].ConnectionString;

            if (!ModelState.IsValid)
            {
                // Dữ liệu nhập không hợp lệ => quay lại form
                return View("Register", model);
            }

            if (model.Key != "123456")
            {
                ModelState.AddModelError("", "Khóa xác thực không hợp lệ!");
                return View("Register", model);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SP_REGISTER", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@USERNAME", model.UserName);
                        cmd.Parameters.AddWithValue("@PASSWORD", model.Password);
                        cmd.Parameters.AddWithValue("@KEY", model.Key);

                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["SuccessMessage"] = "Đăng ký tài khoản thành công!";
                return RedirectToAction("Login", "Admin");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại hoặc lỗi SQL: " + ex.Message);
                return View("Register", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi không xác định: " + ex.Message);
                return View("Register", model);
            }
        }
    }
}