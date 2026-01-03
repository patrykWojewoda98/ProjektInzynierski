using ProjektIznynierski.Domain.Abstractions;
using System.Net;
using System.Net.Mail;

namespace ProjektIznynierski.Infrastructure.Services
{
    internal class EmailService : IEmailService
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _from;

        public EmailService()
        {
            _host = Environment.GetEnvironmentVariable("SMTP_HOST")
                ?? throw new Exception("SMTP_HOST not set");

            _port = int.Parse(
                Environment.GetEnvironmentVariable("SMTP_PORT")
                ?? throw new Exception("SMTP_PORT not set"));

            _username = Environment.GetEnvironmentVariable("SMTP_USERNAME")
                ?? throw new Exception("SMTP_USERNAME not set");

            _password = Environment.GetEnvironmentVariable("SMTP_PASSWORD")
                ?? throw new Exception("SMTP_PASSWORD not set");

            _from = Environment.GetEnvironmentVariable("SMTP_FROM")
                ?? throw new Exception("SMTP_FROM not set");
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = true
            };

            var mail = new MailMessage(_from, to, subject, body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mail);
        }
    }
}
