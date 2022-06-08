using RestAPIDemo.Models;
using RestAPIDemo.Repositories.RefreshTokenReopsitory;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIDemo.Repositories.RefreshTokenRepository
{
    public class DbRefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AuthenticationDbContext _context;

        public DbRefreshTokenRepository(AuthenticationDbContext context)
        {
            _context = context;
        }

        public async Task Create(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(Guid id)
        {
            RefreshToken refreshToken = await _context.RefreshTokens.FindAsync(id);

            if(refreshToken != null)
            {
                _context.RefreshTokens.Remove(refreshToken);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAll(Guid userId)
        {
            IEnumerable<RefreshToken> refreshTokens = await _context.RefreshTokens.Where
                (r => r.UserId == userId).ToListAsync();         

            _context.RefreshTokens.RemoveRange(refreshTokens);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetByToken(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(refresh => refresh.Token == token);
        }
    }
}
