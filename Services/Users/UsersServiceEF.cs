using DotNetCardsServer.Interfaces;
using DotNetCardsServer.Models.Users;
using DotNetCardsServer.Services.Data;

namespace DotNetCardsServer.Services.Users
{
    public class UsersServiceEF 
    {
        private readonly ApplicationDbContext _context;
        public UsersServiceEF(ApplicationDbContext context)
        {
            _context = context;
        }

        //Task<object> CreateUserAsync(User newUser) { } // sultan
        //Task<List<User>> GetUsersAsync() { } //itay
        //Task<User> GetOneUserAsync(string userId) { } //yotam
        //Task DeleteUserAsync(string userId) { } //tamir
        //Task<User> EditUserAsync(string userId, User updatedUser) { } 
        //Task<User> LoginAsync(LoginModel loginModel) { }

    }
}
