using System;
using System.Data.SqlClient;

namespace StackOverflow.Models
{
    public class Database
    {
        private static SqlConnection _conn = null;
        
        // Admin login
        public SqlConnection Connection(string username, string password)
        {
            try
            {
                string connString = $"Server=tcp:stackoverflowvn-sql.database.windows.net,1433;Initial Catalog=ForumDB;Persist Security Info=False;User ID={username};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    return conn;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connect failed: " + ex.Message);
                return null;
            }
        }
        
        // User login
        public SqlConnection Connection()
        {
            try
            {
                string connString = "Server=.;Database=Forum;User Id=sa;Password=sa;";
                if (_conn == null)
                {
                    _conn = new SqlConnection(connString);
                    _conn.Open();
                }
                return _conn;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connect failed: " + ex.Message);
                return null;
            }
        }
    }
}