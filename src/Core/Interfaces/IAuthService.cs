namespace Core.Interfaces
{
    public interface IAuthService 
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerfyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
