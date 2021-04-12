using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models;

namespace Aroma_Shop.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public bool AddMessage(Message message)
        {
            try
            {
                _messageRepository.AddMessage(message);
                _messageRepository.Save();
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public IEnumerable<Message> GetMessages()
        {
            return _messageRepository.GetMessages();
        }
    }
}
