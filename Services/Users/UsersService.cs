using DotNetCardsServer.Models.Users;
using MongoDB.Driver;

namespace DotNetCardsServer.Services.Users
{
    public class UsersService
    {
        private  IMongoCollection<User> _users;

        public UsersService(IMongoClient mongoClient) 
        {
            var database = mongoClient.GetDatabase("dotnet_business_card_app");
            _users = database.GetCollection<User>("users");
        }

        public async Task<User> CreateUserAsync (User newUser)
        {
            var existingUser = await _users.Find(u => u.Email == newUser.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }
            await _users.InsertOneAsync(newUser);
            return newUser;
        }

        //get Users
        // get one user
        //delete user
        //edit user
        //login
    }
}
