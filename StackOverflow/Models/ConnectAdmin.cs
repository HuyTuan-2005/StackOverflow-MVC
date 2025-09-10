using System;
using System.Data.SqlClient;

namespace StackOverflow.Models
{
    public class ConnectAdmin
    {
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
                Console.WriteLine("Kết nối thất bại: " + ex.Message);
                return null;
            }
        }
    }
}