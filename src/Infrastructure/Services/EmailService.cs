using Core.Enums;
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
        public bool SendEmail(string userName, string to, EmailType type)
        {
            try
            {    
                var email = FormatEmail(userName, to, type);
                
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

        public bool SendEmail(string userName, string to, string from, string subject, string body)
        {
            try
            {
                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(from));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };

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

        private MimeMessage FormatEmail(string userName, string to, EmailType type)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("no-reply@infusync.com"));
            email.To.Add(MailboxAddress.Parse(to));
            if (type == EmailType.Register)
            {
                email.Subject = $"Email Confirmation";
                string html = $"<p> Hello {userName},</p><br><p> Thanks for getting started with us. <strong>Infusync HM</strong>";
                email.Body = new TextPart(TextFormat.Html) { Text = html };
            }
            else if(type == EmailType.Reservation)
            {
                email.Subject = $"Reservation Email";
                string html = $"<p> Hello {userName},</p><br><p> Room has been resvered for you. <strong>Infusync HM</strong>";
                email.Body = new TextPart(TextFormat.Html) { Text = html };
            }
            return email;
        }
    }
}