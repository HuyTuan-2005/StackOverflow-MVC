using System;
using System.Configuration;
using System.Data.SqlClient;

namespace StackOverflow.Models
{
    public class Database
    {
        public static string ConnString = ConfigurationManager.ConnectionStrings["ForumDB"].ConnectionString;
        public static SqlConnection _conn = null;
        
        // Admin login
        public SqlConnection Connection(string username, string password)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(ConnString);
                builder.UserID = username;
                builder.Password = password;
                ConnString = builder.ConnectionString;
                
                using (var conn = new SqlConnection(ConnString))
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
                if (_conn == null)
                {
                    _conn = new SqlConnection(ConnString);
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