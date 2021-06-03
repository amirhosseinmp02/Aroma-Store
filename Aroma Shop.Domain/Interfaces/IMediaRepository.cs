using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.MediaModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IMediaRepository : IGeneralRepository
    {
        Comment GetComment(int commentId);
        void UpdateComment(Comment comment);
        IEnumerable<Comment> GetComments();
        int GetUnreadCommentsCount();
        void DeleteComment(Comment comment);
        void DeleteCommentById(int commentId);
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Message GetMessage(int messageId);
        IEnumerable<Message> GetMessages();
        int GetUnreadMessagesCount();
    }
}
