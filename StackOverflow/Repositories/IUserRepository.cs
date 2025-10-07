using StackOverflow.Models;
using StackOverflow.ViewModels;

namespace StackOverflow.Repositories
{
    public interface IUserRepository
    {
        int VerifyUser(UserLoginViewModel user);

        int CheckValidUser(UserRegisterViewModel user);

        int Register(UserRegisterViewModel user);
    }
}