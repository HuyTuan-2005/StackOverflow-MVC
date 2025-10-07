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
                
                // SqlCommand([cmdText], connection)
                // cmdText = tên stored procedure
                using (var command = new SqlCommand("sp_VerifyUser", conn))
                {
                    // CommandType = StoredProcedure sử dụng stored procedure trong SQL Server
                    command.CommandType = CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@username", user.UserName);
                    command.Parameters.AddWithValue("@password", user.Password);
                    
                    // khai báo giá trị nhận output trong sql server
                    // SqlParameter([tenthuoctinh output], [datatype in db])
                    SqlParameter result = new SqlParameter("@result", SqlDbType.Int);
                    
                    // Direction = Output để nhan gia tri tra ve tu stored procedure
                    result.Direction = ParameterDirection.Output;
                    command.Parameters.Add(result);
                    
                    command.ExecuteNonQuery();
                    
                    // giá trị output = result.value
                    return (int)result.Value;
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