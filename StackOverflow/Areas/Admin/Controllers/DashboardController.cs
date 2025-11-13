using StackOverflow.Areas.Admin.Data;
using StackOverflow.Helpers;
using StackOverflow.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


namespace StackOverflow.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            // var parameters = ThongKeDashboard();
            using (SqlConnection conn = DbFactory.GetConnection())
            {
                conn.Open();
                var command = new SqlCommand("sp_getAllProfile", conn);
                command.CommandType = CommandType.StoredProcedure;

                List<Profile> lstProfile = new List<Profile>();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var profile = new Profile()
                        {
                            DisplayName = reader["Display_Name"].ToString(),
                            Birthday = Convert.ToDateTime(reader["Birthday"] == DBNull.Value ? DateTime.Now : reader["Birthday"]),
                            Gender = reader["Gender"] == DBNull.Value ? "Chưa xác định" : reader["Gender"].ToString(),
                        };
                        lstProfile.Add(profile);
                    }
                }
                return View(lstProfile);
            }
        }


        public ActionResult Export()
        {
            try
            {
                string tableName = "QUESTIONS";
                string tempFile = Path.Combine(Path.GetTempPath(), $"{tableName}_export.csv");

                using (SqlConnection conn = DbFactory.GetConnection())
                {
                    conn.Open();

                    string query = $"SELECT * FROM {tableName}";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    using (StreamWriter writer = new StreamWriter(tempFile, false, new UTF8Encoding(true))) // ✅ UTF-8 BOM
                    {
                        // --- Ghi dòng tiêu đề ---
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (i > 0) writer.Write(",");
                            string columnName = reader.GetName(i)?.Replace("\"", "\"\"");
                            writer.Write($"\"{columnName}\"");
                        }
                        writer.WriteLine();

                        // --- Ghi dữ liệu ---
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (i > 0) writer.Write(",");
                                string value = reader[i]?.ToString() ?? "";
                                value = value.Replace("\"", "\"\"");
                                writer.Write($"\"{value}\"");
                            }
                            writer.WriteLine();
                        }
                    }
                }

                // --- Đọc file ra mảng byte ---
                byte[] fileBytes = System.IO.File.ReadAllBytes(tempFile);

                // --- Xoá file tạm ---
                if (System.IO.File.Exists(tempFile))
                    System.IO.File.Delete(tempFile);

                string outputFileName = $"{tableName}_export.csv";

                // --- Trả file ra client ---
                return File(fileBytes, "text/csv; charset=utf-8", outputFileName);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi khi export: " + ex.Message;
                return View("Dashboard");
            }
        }


        private string EscapeCsv(string input)
        {
            if (input.Contains(",") || input.Contains("\"") || input.Contains("\n") || input.Contains("\r"))
            {
                input = input.Replace("\"", "\"\"");
                return $"\"{input}\"";
            }
            return input;
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                {
                    ViewBag.Error = "Vui lòng chọn file CSV hợp lệ.";
                    return View("Dashboard");
                }

                string tableName = "QUESTIONS";
                string tempFile = Path.Combine(Path.GetTempPath(), $"{tableName}_import.csv");
                file.SaveAs(tempFile);

                DataTable dataTable = ReadCsvToDataTable(tempFile);

                string connectionString = ConfigurationManager.ConnectionStrings["ForumDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Lấy cột tự tăng
                    List<string> identityColumns = new List<string>();
                    string getIdentityQuery = $"SELECT name FROM sys.identity_columns WHERE object_id = OBJECT_ID('{tableName}')";
                    using (SqlCommand cmd = new SqlCommand(getIdentityQuery, conn))
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                            identityColumns.Add(rdr.GetString(0));
                    }

                    // Loại bỏ cột IDENTITY khỏi DataTable
                    foreach (var col in identityColumns)
                        if (dataTable.Columns.Contains(col))
                            dataTable.Columns.Remove(col);

                    // Import USERS
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                    {
                        bulkCopy.DestinationTableName = tableName;
                        foreach (DataColumn col in dataTable.Columns)
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        bulkCopy.WriteToServer(dataTable);
                    }

                }

                if (System.IO.File.Exists(tempFile))
                    System.IO.File.Delete(tempFile);

                ViewBag.Message = "✅ Import thành công và đã tạo hồ sơ tự động!";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "❌ Lỗi khi import: " + ex.Message;
                return View("Dashboard");
            }
        }

        private DataTable ReadCsvToDataTable(string filePath)
        {
            DataTable dt = new DataTable();

            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                bool headerRead = false;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    // ✅ Regex tách cột, giữ nguyên dấu phẩy trong chuỗi
                    var values = Regex.Matches(line, "(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)")
                                      .Cast<Match>()
                                      .Select(m => m.Value.Trim().Trim('"').Replace("\"\"", "\""))
                                      .ToArray();

                    if (!headerRead)
                    {
                        foreach (var col in values)
                            dt.Columns.Add(col);
                        headerRead = true;
                    }
                    else
                    {
                        dt.Rows.Add(values);
                    }
                }
            }

            return dt;
        }

        public SqlParameter[] ThongKeUser()
        {
            using (var conn = DbFactory.GetConnection())
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

        public ActionResult CountUser()
        {
            using (SqlConnection conn = DbFactory.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_CountUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ViewBag.TotalUser = Convert.ToInt32(reader["TotalUser"]);
                        //ViewBag.ActiveUser = Convert.ToInt32(reader["ActiveUser"]);
                        //ViewBag.NewUserToday = Convert.ToInt32(reader["NewUserToday"]);
                        //ViewBag.ActiveRate = Convert.ToDouble(reader["ActiveRate"]);
                    }
                }
            }

            return PartialView("ThongKeUser");
        }

    }
}