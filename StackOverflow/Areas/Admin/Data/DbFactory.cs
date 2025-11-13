using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StackOverflow.Areas.Admin.Data
{
    public class DbFactory
    {
        private string _connectionString;
        public string ConnectionString // Public property
        {
            get { return _connectionString; }
            set { ConnectionString = value; }
        }

        public DbFactory()
        {
            _connectionString = System.Web.HttpContext.Current?.Session?["UserConnStr"] as string;
        }

        public static SqlConnection GetConnection()
        {
            var connStr = System.Web.HttpContext.Current?.Session?["UserConnStr"] as string;
            return string.IsNullOrEmpty(connStr) ? null : new SqlConnection(connStr);
        }

        // Trả về DataTable cho truy vấn SELECT
        public DataTable ExecuteQuery(string query)
        {
            DataTable result = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(result);
                    }
                }
            }

            return result;
        }

        // Thực thi câu lệnh không trả về (INSERT, UPDATE, DELETE)
        public int ExecuteNonQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}