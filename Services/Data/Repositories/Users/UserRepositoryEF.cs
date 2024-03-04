using DotNetCardsServer.Exceptions;
using DotNetCardsServer.Models.Users;
using DotNetCardsServer.Services.Data.Repositories.Intrefaces;
using Microsoft.EntityFrameworkCore;

namespace DotNetCardsServer.Services.Data.Repositories.Users
{
    public class UserRepositoryEF: IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepositoryEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUserAsync(User newUser)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
            if (existingUser != null)
            {
                return false;
            }
            UserSqlModel normalizedUser = new UserSqlModel(newUser);
            await _context.Users.AddAsync(normalizedUser);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<User>> GetAllUsersAsync(bool includePassword = false)
        {
            var userSqlModels = await _context.Users.ToListAsync();
            if (includePassword)
            {
                var users = userSqlModels.Select(u => new User(u)).ToList();

                return users;
            }
            else
            {
                var users = userSqlModels.Select(u => new User(u)).ToList();
                users.ForEach(u => u.Password = "");
                return users;
            }
            

        }

        public async Task<User> GetOneUserAsync(string userId, bool includePassword = false)
        {
            var user = await _context.Users
              .Where(u => u.Id == userId)
              .FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            if (!includePassword)
            {
                user.Password = "";
            }

            var oneUser = new User(user);

            return oneUser;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> EditUserAsync(string userId, User updatedUser)
        {
            var userSqlModel = await _context.Users.FindAsync(userId);
            if (userSqlModel == null)
            {
                return null;
            }

            // Update fields in UserSqlModel
            userSqlModel.FirstName = updatedUser.Name.First;
            userSqlModel.LastName = updatedUser.Name.Last;
            userSqlModel.MiddleName = updatedUser.Name.Middle;
            userSqlModel.Email = updatedUser.Email;
            userSqlModel.Phone = updatedUser.Phone;
            userSqlModel.IsBusiness = updatedUser.IsBusiness;
            userSqlModel.IsAdmin = updatedUser.IsAdmin;
            userSqlModel.ImageUrl = updatedUser.Image.Url;
            userSqlModel.ImageAlt = updatedUser.Image.Alt;

            // Address
            userSqlModel.State = updatedUser.Address.State;
            userSqlModel.Country = updatedUser.Address.Country;
            userSqlModel.City = updatedUser.Address.City;
            userSqlModel.Street = updatedUser.Address.Street;
            userSqlModel.HouseNumber = updatedUser.Address.HouseNumber;
            userSqlModel.Zip = updatedUser.Address.Zip;

            // Save changes in the database
            await _context.SaveChangesAsync();

            // Convert back to User type before returning, using the User constructor that accepts a UserSqlModel
            return new User(userSqlModel);
        }
        public async Task<User> GetUserByEmail(string userEmail)
        {
            var user = await _context.Users
              .FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null) return null;
            return new User(user);
        }
    }
}
