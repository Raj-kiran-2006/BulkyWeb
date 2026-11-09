using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace Bulky.Utility
{
    public class Email_Sender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public Email_Sender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var host = GetRequiredSetting("Host");
            var username = GetRequiredSetting("Username");
            var password = GetRequiredSetting("Password");
            var fromEmail = GetRequiredSetting("FromEmail");
            var fromName = GetRequiredSetting("FromName");

            if (!int.TryParse(_configuration["Smtp:Port"], out var port) || port is < 1 or > 65535)
            {
                throw new InvalidOperationException("SMTP port is invalid or not configured.");
            }

            if (!bool.TryParse(_configuration["Smtp:EnableSsl"], out var enableSsl))
            {
                throw new InvalidOperationException("SMTP SSL setting is invalid or not configured.");
            }

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password)
            };

            using var message = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            message.To.Add(email);

            await client.SendMailAsync(message);
        }

        private string GetRequiredSetting(string name)
        {
            return _configuration[$"Smtp:{name}"]
                ?? throw new InvalidOperationException($"SMTP setting '{name}' is not configured.");
        }
    }
}
