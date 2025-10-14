using System;
using System.Configuration;
using System.Data.SqlClient;

namespace StackOverflow.Models
{
    public class Database
    {
        public static string ConnString = ConfigurationManager.ConnectionStrings["ForumDB"].ConnectionString;

        // Admin login
        public SqlConnection Connection(string username, string password)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(ConnString);
                builder.UserID = username;
                builder.Password = password;
                ConnString = builder.ConnectionString;

                var conn = new SqlConnection(ConnString);
                conn.Open();
                return conn;
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
                var conn = new SqlConnection(ConnString);
                conn.Open();

                return conn;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connect failed: " + ex.Message);
                return null;
            }
        }
    }
}