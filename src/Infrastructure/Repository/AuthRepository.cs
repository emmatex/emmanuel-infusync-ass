using Core.Entities;
using Core.Interfaces;
using Infrastructure.Helpers;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AuthRepository : GenericRepository<User>, IAuthRepository
    {
        public AuthRepository(MongoDbSettings settings) : base(settings)
        {
        }

        public Task<User> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<User> Register(User user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
