using DotNetCardsServer.Exceptions;
using DotNetCardsServer.Interfaces;
using DotNetCardsServer.Models.Users;
using DotNetCardsServer.Services.Data;
using DotNetCardsServer.Utils;
using Microsoft.EntityFrameworkCore;

namespace DotNetCardsServer.Services.Users
{
    public class UsersServiceEF :IUsersService
    {
        private readonly ApplicationDbContext _context;
        public UsersServiceEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> CreateUserAsync(User newUser) {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException("User with this email already exists.");
            }

            newUser.Password = PasswordHelper.GeneratePassword(newUser.Password);

            UserSqlModel normalizedUser = new UserSqlModel(newUser);

            await _context.Users.AddAsync(normalizedUser);
            await _context.SaveChangesAsync();

            return new { newUser.Id, newUser.Name, newUser.Email };
        }


        public async Task<List<User>> GetUsersAsync()
        {
            var userSqlModels = await _context.Users.ToListAsync();
            var users = userSqlModels.Select(u => new User(u)).ToList();

            return users;
        }

        public async Task<User> GetOneUserAsync(string userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            

            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            var oneUser = new User(user);

            return oneUser;
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }


        public async Task<User> EditUserAsync(string userId, User updatedUser)
        {
            var userSqlModel = await _context.Users.FindAsync(userId);
            if (userSqlModel == null)
            {
                throw new UserNotFoundException("User with ID: " + userId + " not found.");
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



        public async Task<User> LoginAsync(LoginModel loginModel)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginModel.Email);

            if (user == null || !PasswordHelper.VerifyPassword(user.Password, loginModel.Password))
            {
                throw new AuthenticationException();
            }

            // Consider not including the password in the returned user object
            user.Password = null;
            return new User(user);
        }
    }
}
