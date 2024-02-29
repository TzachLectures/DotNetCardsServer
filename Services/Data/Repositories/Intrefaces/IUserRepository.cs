using DotNetCardsServer.Models.Users;

namespace DotNetCardsServer.Services.Data.Repositories.Intrefaces
{
    public interface IUserRepository
    {
         Task<bool> CreateUserAsync(User newUser);
         Task<List<User>> GetAllUsersAsync(bool includePassword=false);
         Task<User> GetOneUserAsync(string userId, bool includePassword = false);
         Task<bool> DeleteUserAsync(string userId);
         Task<User> EditUserAsync(string userId, User updatedUser);
         Task<User> GetUserByEmail(string userId);
    }
}
