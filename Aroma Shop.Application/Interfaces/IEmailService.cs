using System.Threading.Tasks;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false);
    }
}
