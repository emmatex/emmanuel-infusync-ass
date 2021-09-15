using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthService 
    {
        string CreateToken(User user);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerfyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
