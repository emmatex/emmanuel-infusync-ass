using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthRepository : IGenericRepository<User>
    {
        Task<User> Login(string email, string password);
        Task<User> Register(User user, string password);
    }
}
