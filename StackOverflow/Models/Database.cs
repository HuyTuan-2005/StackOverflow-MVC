using System;
using System.Configuration;
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
                string connString = ConfigurationManager.ConnectionStrings["ForumDB"].ConnectionString;
                var builder = new SqlConnectionStringBuilder(connString);
                builder.UserID = username;
                builder.Password = password;
                connString = builder.ConnectionString;
                
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
                string connString = ConfigurationManager.ConnectionStrings["ForumDB"].ConnectionString;
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