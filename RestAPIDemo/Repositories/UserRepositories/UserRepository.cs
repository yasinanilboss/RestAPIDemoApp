using RestAPIDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIDemo.Repositories.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();
        public Task<User> CreateUser(User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);

            return Task.FromResult(user);
        }

        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(user => user.Email == email));
        }

        public Task<User> GetById(Guid userId)
        {
            return Task.FromResult(_users.FirstOrDefault(user => user.Id == userId));
        }

        public Task<User> GetByUsername(string username)
        {
            return Task.FromResult(_users.FirstOrDefault(user => user.Username == username));
        }
    }
}
