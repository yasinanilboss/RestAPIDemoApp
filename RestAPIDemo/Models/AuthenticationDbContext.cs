using Microsoft.EntityFrameworkCore;

namespace RestAPIDemo.Models
{
    public class AuthenticationDbContext : DbContext
    {

        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
