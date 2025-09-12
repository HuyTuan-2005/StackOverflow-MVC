using System.Data.SqlClient;

namespace StackOverflow.Models
{
    public class Database
    {
        private static SqlConnection _conn = null;
        
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
            catch (SqlException e)
            {
                return null;
            }
        }
    }
}