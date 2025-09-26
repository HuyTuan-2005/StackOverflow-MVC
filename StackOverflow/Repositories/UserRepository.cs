using System.Configuration;
using System.Data.SqlClient;
using StackOverflow.Models;

namespace StackOverflow.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connString;

        public UserRepository(string connString)
        {
            _connString = connString;
        }
        
        public User GetById(int id)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var command = new SqlCommand("sp_GetUserByID", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User()
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2),
                                Email = reader.GetString(3),
                                CreatedAt = reader.GetDateTime(4),
                                LastActivity = reader.GetDateTime(5),
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}