using RestAPIDemo.Models;
using System;
using System.Threading.Tasks;

namespace RestAPIDemo.Repositories.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByUsername(string username);
        Task<User> CreateUser(User user);
        Task<User> GetById(Guid userId);
    }
}
