using StackOverflow.Models;

namespace StackOverflow.Services
{
    public interface IUserService
    {
        User GetUser(int id);
    }
}