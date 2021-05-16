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
                message.SubmitTime = DateTime.Now;

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

        public bool DeleteMessageById(int messageId)
        {
            try
            {
                var message = GetMessage(messageId);

                if (message == null)
                    return false;

                _messageRepository.DeleteMessage(message);
                _messageRepository.Save();
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public Message GetMessage(int messageId)
        {
            var message = _messageRepository.GetMessage(messageId);

            return message;
        }

        public IEnumerable<Message> GetMessages()
        {
            return _messageRepository.GetMessages();
        }
    }
}
