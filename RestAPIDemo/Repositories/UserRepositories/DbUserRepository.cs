using RestAPIDemo.Models;
using System;
// using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIDemo.Repositories.UserRepositories
{
    public class DbUserRepository : IUserRepository
    {
        private readonly AuthenticationDbContext _context;

        public DbUserRepository(AuthenticationDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        }

        public async Task<User> GetById(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
