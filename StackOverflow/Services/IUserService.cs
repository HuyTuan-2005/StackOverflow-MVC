using StackOverflow.Models;
using StackOverflow.ViewModels;

namespace StackOverflow.Services
{
    public interface IUserService
    {
        int VerifyUser(UserLoginViewModel user);
    }
}