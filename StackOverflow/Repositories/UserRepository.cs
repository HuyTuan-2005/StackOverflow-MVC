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

        public int VerifyUser(UserLoginViewModel user)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var command = new SqlCommand("sp_VerifyUser", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@username", user.UserName);
                    command.Parameters.AddWithValue("@password", user.Password);
                    
                    
                    var result = new SqlParameter("@result", SqlDbType.Int);
                    result.Direction = ParameterDirection.Output;
                    command.Parameters.Add(result);
                    
                    command.ExecuteNonQuery();
                    return (int)result.Value;
                }
            }
        }
    }
}