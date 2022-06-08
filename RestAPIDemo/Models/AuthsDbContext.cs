using Microsoft.EntityFrameworkCore;

namespace RestAPIDemo.Models
{
    public class AuthsDbContext : DbContext
    {

        public AuthsDbContext(DbContextOptions<AuthsDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
