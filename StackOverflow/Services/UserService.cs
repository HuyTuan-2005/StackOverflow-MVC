using StackOverflow.Models;
using StackOverflow.Repositories;
using StackOverflow.ViewModels;


namespace StackOverflow.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int VerifyUser(UserLoginViewModel user)
        {
            return _userRepository.VerifyUser(user);
        }
    }
}