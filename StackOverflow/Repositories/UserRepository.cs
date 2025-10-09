using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Ajax.Utilities;
using StackOverflow.Models;
using StackOverflow.ViewModels;

namespace StackOverflow.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connString;

        public UserRepository(string connString)
        {
            _connString = connString;
        }

        public User VerifyUser(string username, string password)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                // SqlCommand([cmdText], connection)
                // cmdText = tên stored procedure
                using (var command = new SqlCommand("sp_VerifyUser", conn))
                {
                    // CommandType = StoredProcedure sử dụng stored procedure trong SQL Server
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);


                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return new User()
                        {
                            UserId = Convert.ToInt32(reader["user_id"]),
                            Email = Convert.ToString(reader["email"]),
                            UserName = Convert.ToString(reader["username"]),
                            Password = null,
                            CreatedAt = Convert.ToDateTime(reader["created_at"]),
                            LastActivity = Convert.ToDateTime(reader["last_activity"])
                        };
                    }
                    return null;
                }
            }
        }

        public int CheckValidUser(UserRegisterViewModel user)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var command = new SqlCommand("select dbo.f_CheckValidUser(@username, @email)", conn))
                {
                    command.Parameters.AddWithValue("@username", user.UserName);
                    command.Parameters.AddWithValue("@email", user.Email);

                    return (int)command.ExecuteScalar();
                }
            }
        }

        public int Register(UserRegisterViewModel user)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var command = new SqlCommand("sp_RegisterUser", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@username", user.UserName);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@password", user.Password);

                    var result = new SqlParameter("@OutUserId", SqlDbType.Int);
                    result.Direction = ParameterDirection.Output;
                    command.Parameters.Add(result);

                    command.ExecuteNonQuery();

                    return (int)result.Value;
                }
            }
        }
    }
}