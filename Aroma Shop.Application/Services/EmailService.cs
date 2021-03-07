using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;

namespace Aroma_Shop.Application.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false)
        {
            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential()
                {
                    UserName = "alliance.000009",
                    Password = "$8amiriiahosseiN0$"
                };
                client.Credentials = credentials;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                using var emailMessage = new MailMessage()
                {
                    To = { new MailAddress(toEmail) },
                    From = new MailAddress("alliance.000009@gmail.com"),
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
