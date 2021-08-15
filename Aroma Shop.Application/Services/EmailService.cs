using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Aroma_Shop.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false)
        {
            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential()
                {
                    UserName = _configuration["EmailSettings:Username"],
                    Password = _configuration["EmailSettings:Password"]
                };

                client.Credentials = credentials;
                client.Host = _configuration["EmailSettings:Host"];
                client.Port = Convert.ToInt32(_configuration["EmailSettings:Port"]);
                client.EnableSsl = true;

                using var emailMessage = new MailMessage()
                {
                    To = { new MailAddress(toEmail) },
                    From = new MailAddress(_configuration["EmailSettings:From"]),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = isMessageHtml
                };
                
                client.Send(emailMessage);
            }

            return Task.CompletedTask;
        }
    }
}
