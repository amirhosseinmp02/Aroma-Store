using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Domain.Models.MessageModels;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IMessageService
    {
        bool AddMessage(Message message);
        bool DeleteMessageById(int messageId);
        Message GetMessage(int messageId);
        IEnumerable<Message> GetMessages();
        int GetUnreadMessagesCount();
        Task<bool> ReplyToMessage(string messageReplyDescription, int messageId);
        bool SetAsRead(Message message);
    }
}
