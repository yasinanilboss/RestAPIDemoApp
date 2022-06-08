using RestAPIDemo.Models;
using System;
using System.Threading.Tasks;

namespace RestAPIDemo.Repositories.RefreshTokenReopsitory
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetByToken(string token);
        Task Create(RefreshToken refreshToken);
        Task Delete(Guid id);
        Task DeleteAll(Guid userId);
    }
}
