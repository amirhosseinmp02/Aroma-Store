using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.Message;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.MessageModels;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IEmailService _emailService;

        public MessageService(IMessageRepository messageRepository, IHttpContextAccessor accessor, IEmailService emailService)
        {
            _messageRepository = messageRepository;
            _accessor = accessor;
            _emailService = emailService;
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
            var message =
                _messageRepository.GetMessage(messageId);

            return message;
        }

        public IEnumerable<Message> GetMessages()
        {
            return _messageRepository.GetMessages();
        }

        public int GetUnreadMessagesCount()
        {
            var getUnreadMessagesCount =
                _messageRepository.GetUnreadMessagesCount();

            return getUnreadMessagesCount;
        }

        public async Task<bool> ReplyToMessage(string messageReplyDescription, int messageId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(messageReplyDescription))
                    return false;

                var message =
                    GetMessage(messageId);

                if (message == null)
                    return false;

                var emailMessageViewModel = new ReplyToMessageEmailTemplateViewModel()
                {
                    MessageSubject = message.MessageSubject,
                    MessageReplyDescription = messageReplyDescription
                };

                var emailMessage =
                    await ViewToStringRenderer
                        .RenderViewToStringAsync(_accessor.HttpContext.RequestServices,
                            "~/Views/Emails/ReplyToMessageTemplate.cshtml", emailMessageViewModel);
                await
                    _emailService.SendEmailAsync
                        (message.MessageSenderEmail, "پاسخ به سوال شما", emailMessage.ToString(), true);

                var messageReply = new MessageReply()
                {
                    MessageReplySubmitTime = DateTime.Now,
                    MessageReplyDescription = messageReplyDescription
                };

                message.MessageReply = messageReply;
                message.IsReplied = true;

                _messageRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public bool SetAsRead(Message message)
        {
            try
            {
                _messageRepository.SetMessageAsRead(message);

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }
    }
}
