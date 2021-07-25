using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.ViewModels.Banner;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Models.MediaModels;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IMediaService
    {
        IEnumerable<Comment> GetComments();
        int GetUnreadCommentsCount();
        Task<bool> AddReplyToCommentByAdmin(int commentId, string newCommentReplyDescription);
        Task<bool> AddCommentToProduct(ProductViewModel productViewModel);
        Task<bool> AddReplyToProductComment(ProductViewModel productViewModel);
        Comment GetComment(int commentId);
        bool UpdateComment(Comment comment);
        bool SetCommentAsRead(Comment comment);
        bool DeleteComment(Comment comment);
        bool DeleteCommentById(int commentId);
        bool ConfirmComment(int commentId);
        bool RejectComment(int commentId);
        bool AddMessage(Message message);
        bool DeleteMessageById(int messageId);
        Message GetMessage(int messageId);
        IEnumerable<Message> GetMessages();
        int GetUnreadMessagesCount();
        Task<bool> ReplyToMessage(string messageReplyDescription, int messageId);
        bool SetMessageAsRead(Message message);
        bool AddBanner(AddEditBannerViewModel bannerViewModel);
    }
}
