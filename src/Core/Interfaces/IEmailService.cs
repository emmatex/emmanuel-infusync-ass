namespace Core.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(string userName, string to);
    }
}
