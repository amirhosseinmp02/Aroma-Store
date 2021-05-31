using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.MessageModels;
using Microsoft.EntityFrameworkCore;

namespace Aroma_Shop.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }
        public void AddMessage(Message message)
        {
            _context.Add(message);
        }
        public void DeleteMessage(Message message)
        {
            _context.Remove(message);
        }
        public Message GetMessage(int messageId)
        {
            var message =
                _context.Messages
                    .Include(p => p.MessageReply)
                    .SingleOrDefault(p => p.MessageId == messageId);

            return message;
        }
        public IEnumerable<Message> GetMessages()
        {
            var messages =
                _context.Messages;

            return messages;
        }
        public int GetUnreadMessagesCount()
        {
            var unreadMessagesCount = 
                _context.Messages.Count(p => !p.IsRead);

            return unreadMessagesCount;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
