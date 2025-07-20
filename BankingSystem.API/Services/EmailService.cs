using BankingSystem.API.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace BankingSystem.API.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_settings.From));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Text) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_settings.Username, _settings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}

