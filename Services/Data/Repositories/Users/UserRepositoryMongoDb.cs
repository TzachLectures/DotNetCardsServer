using DotNetCardsServer.Exceptions;
using DotNetCardsServer.Models.Users;
using MongoDB.Driver;

namespace DotNetCardsServer.Services.Data.Repositories.Users
{
    public class UserRepositoryMongoDb
    {
        private IMongoCollection<User> _users;

        public UserRepositoryMongoDb(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("dotnet_business_card_app");
            _users = database.GetCollection<User>("users");
        }
        public async Task<bool> CreateUserAsync(User newUser)
        {
            var existingUser = await _users.Find(u => u.Email == newUser.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return false;
            }
            await _users.InsertOneAsync(newUser);
            return true;

        }

    }
}
