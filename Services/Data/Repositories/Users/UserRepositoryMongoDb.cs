using DotNetCardsServer.Exceptions;
using DotNetCardsServer.Models.Users;
using DotNetCardsServer.Services.Data.Repositories.Intrefaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DotNetCardsServer.Services.Data.Repositories.Users
{
    public class UserRepositoryMongoDb: IUserRepository
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

        public async Task<List<User>> GetAllUsersAsync(bool includePassword = false)
        {
            var builder = Builders<User>.Projection;
            var projection = builder.Exclude("Password");
            if (includePassword)
            {
                List<User> allUsers = await _users.Find(_ => true).ToListAsync();
                return allUsers;
            }
            else
            {
                List<User> allUsers = await _users.Find(_ => true).Project<User>(projection).ToListAsync();
                return allUsers;
            }
           
        }
        public async Task<User> GetOneUserAsync(string userId, bool includePassword = false)
        {
            
            User specificUser = await _users.Find(u => u.Id.ToString() == userId).FirstOrDefaultAsync();

            if (specificUser == null)
            {
                // user not found 
                return null;
            }

            if (!includePassword)
            {
                specificUser.Password = "";
            }
          
           
            return specificUser;
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var result = await _users.DeleteOneAsync(u => u.Id.ToString() == userId);
            return (result.DeletedCount > 0);
           
        }
        public async Task<User> EditUserAsync(string userId, User updatedUser)
        {

            var filter = Builders<User>.Filter.Eq(u => u.Id, new ObjectId(userId));

            var update = Builders<User>.Update
                .Set(u => u.Name, updatedUser.Name)
                .Set(u => u.Email, updatedUser.Email)
                .Set(u => u.Address, updatedUser.Address)
                .Set(u => u.Phone, updatedUser.Phone)
                .Set(u => u.IsBusiness, updatedUser.IsBusiness)
                .Set(u => u.IsAdmin, updatedUser.IsAdmin)
                .Set(u => u.Image, updatedUser.Image);

            var result = await _users.UpdateOneAsync(filter, update);


            // Check if the update was successful
            if (result.MatchedCount == 0)
            {
                return null;
            }

            
            return updatedUser;
        }

        public async Task<User> GetUserByEmail(string userEmail)
        {
            var userLogin = await _users.Find(u => u.Email == userEmail).FirstOrDefaultAsync();
            return userLogin;
        }
    }
}
