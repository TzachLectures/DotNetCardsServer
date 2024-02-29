using DotNetCardsServer.Exceptions;
using DotNetCardsServer.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace DotNetCardsServer.Services.Data.Repositories.Users
{
    public class UserRepositoryEF
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
    }
}
