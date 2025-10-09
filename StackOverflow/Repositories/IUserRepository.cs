using StackOverflow.Models;
using StackOverflow.ViewModels;

namespace StackOverflow.Repositories
{
    public interface IUserRepository
    {
        User VerifyUser(string username, string password);

        int CheckValidUser(UserRegisterViewModel user);

        int Register(UserRegisterViewModel user);
    }
}