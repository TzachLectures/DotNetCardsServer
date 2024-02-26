using DotNetCardsServer.Models.Users;

namespace DotNetCardsServer.Interfaces
{
    public interface IUsersService
    {
        Task<object> CreateUserAsync(User newUser);
        Task<List<User>> GetUsersAsync();
        Task<User> GetOneUserAsync(string userId);
        Task DeleteUserAsync(string userId);
        Task<User> EditUserAsync(string userId, User updatedUser);
        Task<User> LoginAsync(LoginModel loginModel);
    }
}
