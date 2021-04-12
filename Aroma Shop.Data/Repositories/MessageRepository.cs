using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models;

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

        public IEnumerable<Message> GetMessages()
        {
            var messages =
                _context.Messages;

            return messages;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
