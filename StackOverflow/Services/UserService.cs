using StackOverflow.Models;
using StackOverflow.Repositories;

namespace StackOverflow.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUser(int id)
        {
            return _userRepository.GetById(id);
        }
    }
}