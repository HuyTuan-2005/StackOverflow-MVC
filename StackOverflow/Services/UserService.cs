using StackOverflow.Helpers;
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
            user.Password = SimpleHmacSha512.ComputeRawKey(user.Password, user.UserName.ToLower());
            return _userRepository.VerifyUser(user);
        }

        public bool CheckValidUser(UserRegisterViewModel user)
        {
            if (_userRepository.CheckValidUser(user) <= 0)
            {
                return false;
            }
            return true;
        }

        public bool Register(UserRegisterViewModel user)
        {
            if (CheckValidUser(user))
            {
                user.Password = SimpleHmacSha512.ComputeRawKey(user.Password, user.UserName.ToLower());
                if (_userRepository.Register(user) > 0)
                    return true;
            }
            return false;
        }
    }
}