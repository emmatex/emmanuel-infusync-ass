using Core.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public bool SendEmail(string userName, string to)
        {
            try
            {
                // create message
                string html = $"<p> Hello {userName},</p><br><p> Thanks for getting started with <strong>Infusync HM</strong>";
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("no-reply@infusync.com"));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = $"Email Confirmation";
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("mackenzie15@ethereal.email", "dYBu5TSXC5zn5wKWrV");
                smtp.Send(email);
                smtp.Disconnect(true);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
