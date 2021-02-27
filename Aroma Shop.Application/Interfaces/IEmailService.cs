using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IEmailService
    {
        public bool SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false);
    }
}
