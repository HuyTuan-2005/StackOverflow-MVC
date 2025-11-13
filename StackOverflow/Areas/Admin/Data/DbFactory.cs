using System.Data;
using System.Data.SqlClient;

namespace StackOverflow.Areas.Admin.Data
{
    public class DbFactory
    {
        private string _connectionString;
        public string ConnectionString // Public property
        {
            get { return _connectionString; }
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
        public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            DataTable result = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(result);
                    }
                }
            }

            return result;
        }
        
        // Thực thi thủ tục lưu trữ và trả về DataTable
        public DataTable ExecuteStoredProcedure(string storedProcName, params SqlParameter[] parameters)
        {
            DataTable result = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

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
        
        public int ExecuteStoredProcedureNonQuery(string storedProcName, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(storedProcName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}