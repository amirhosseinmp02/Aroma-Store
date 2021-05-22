using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.MessageModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Message GetMessage(int messageId);
        IEnumerable<Message> GetMessages();
        int GetUnreadMessagesCount();
        void SetMessageAsRead(Message message);
        void Save();
    }
}
