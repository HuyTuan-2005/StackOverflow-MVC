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
                string connString = $"Server=localhost;Database=Forum;User Id={username};Password={password};";
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
                //string connString = "Server=192.168.2.10;Database=Forum;User Id=sa;Password=sa;";
                string connString = "Server=localhost\\SQLEXPRESS;Database=Forum;User Id=sa;Password=sa;";
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