using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        IEnumerable<Message> GetMessages();
        void Save();
    }
}
