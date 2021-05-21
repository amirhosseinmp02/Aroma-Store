using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IMessageService
    {
        bool AddMessage(Message message);
        bool DeleteMessageById(int messageId);
        Message GetMessage(int messageId);
        IEnumerable<Message> GetMessages();
        int GetUnreadMessagesCount();
    }
}
