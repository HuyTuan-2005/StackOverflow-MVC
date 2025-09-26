using StackOverflow.Models;

namespace StackOverflow.Repositories
{
    public interface IUserRepository
    {
        User GetById(int id);
    }
}