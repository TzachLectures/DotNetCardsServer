using DotNetCardsServer.Models.Users;

namespace DotNetCardsServer.Services.Data.Repositories.Intrefaces
{
    public interface IUserRepository
    {
         Task<bool> CreateUserAsync(User newUser);
         Task<List<User>> GetAllUsersAsync(bool includePassword=false); // sultan - Mongo, yotam -EF
         Task<User> GetOneUserAsync(string userId, bool includePassword = false);//Yazan - mongo, yaniv-ef
         Task<bool> DeleteUserAsync(string userId);// tamir - mongo, itay - ef
         Task<User> EditUserAsync(string userId, User updatedUser);
         Task<User> GetUserByEmail(string userEmail);
    }
}
