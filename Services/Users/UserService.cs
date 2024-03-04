using DotNetCardsServer.Exceptions;
using DotNetCardsServer.Interfaces;
using DotNetCardsServer.Models.Users;
using DotNetCardsServer.Services.Data;
using DotNetCardsServer.Services.Data.Repositories.Intrefaces;
using DotNetCardsServer.Utils;
using Microsoft.EntityFrameworkCore;

namespace DotNetCardsServer.Services.Users
{
    public class UserService:IUsersService
    {

        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<object> CreateUserAsync(User newUser)
        {
            newUser.Password = PasswordHelper.GeneratePassword(newUser.Password);

            bool result = await _userRepository.CreateUserAsync(newUser);
            if (result)
            {
                return new { newUser.Id, newUser.Name, newUser.Email };
            }
            else
            {
                throw new UserAlreadyExistsException("User with this email already exists.");
            }
        }


        public async Task<List<User>> GetUsersAsync()
        {
          return   await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetOneUserAsync(string userId)
        {
            User user = await _userRepository.GetOneUserAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            return user;
        }

        public async Task DeleteUserAsync(string userId)
        {
            bool result = await _userRepository.DeleteUserAsync(userId);
            if (!result)
            {
                throw new UserNotFoundException(userId);
            }
        }


        public async Task<User> EditUserAsync(string userId, User updatedUser)
        {
            User user = await _userRepository.EditUserAsync(userId, updatedUser);
            if (user == null)
            {
                throw new UserNotFoundException("User with ID: " + userId + " not found.");
            }
            return user;
        }



        public async Task<User> LoginAsync(LoginModel loginModel)
        {
            var user = await _userRepository.GetUserByEmail(loginModel.Email);

            if (user == null || !PasswordHelper.VerifyPassword(user.Password, loginModel.Password))
            {
                throw new AuthenticationException();
            }

            // Consider not including the password in the returned user object
            user.Password = null;
            return user;
        }
    }
}
