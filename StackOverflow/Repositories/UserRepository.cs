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
                
                // cmdText = tên stored procedure
                using (var command = new SqlCommand("sp_VerifyUser", conn))
                {
                    // CommandType = StoredProcedure sử dụng stored procedure trong SQL Server
                    command.CommandType = CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@username", user.UserName);
                    command.Parameters.AddWithValue("@password", user.Password);
                    
                    // khai báo giá trị output trong sql server
                    var result = new SqlParameter("@result", SqlDbType.Int);
                    
                    // Direction = Output
                    result.Direction = ParameterDirection.Output;
                    command.Parameters.Add(result);
                    
                    command.ExecuteNonQuery();
                    
                    // giá trị output = result.value
                    return (int)result.Value;
                }
            }
        }
    }
}