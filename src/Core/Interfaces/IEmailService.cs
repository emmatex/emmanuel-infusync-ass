using Core.Enums;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(string userName, string to, EmailType type);
        bool SendEmail(string userName, string to, string from, string subject, string body);

    }
}
