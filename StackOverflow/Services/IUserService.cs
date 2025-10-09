using StackOverflow.Models;
using StackOverflow.ViewModels;

namespace StackOverflow.Services
{
    public interface IUserService
    {
        User VerifyUser(string username, string password);
        
        bool CheckValidUser(UserRegisterViewModel user);

        bool Register(UserRegisterViewModel user);
    }
}