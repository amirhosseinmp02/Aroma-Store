using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.ViewModels.Banner;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Models.MediaModels;
using Microsoft.AspNetCore.Mvc;

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
        bool DeleteCommentsByParents(IEnumerable<Comment> comments);
        bool DeleteComment(Comment comment);
        bool DeleteCommentById(int commentId);
        bool ConfirmComment(int commentId);
        bool RejectComment(int commentId);
        bool AddMessage(Message message);
        bool DeleteMessageById(int messageId);
        IEnumerable<Message> GetMessages();
        int GetMessagesCount();
        Message GetMessage(int messageId);
        int GetUnreadMessagesCount();
        Task<bool> ReplyToMessage(string messageReplyDescription, int messageId);
        bool SetMessageAsRead(Message message);
        IEnumerable<Banner> GetBanners();
        EditBannerViewModel GetBannerForEdit(int bannerId);
        bool AddBanner(AddBannerViewModel bannerViewModel);
        bool UpdateBanner(EditBannerViewModel bannerViewModel);
        bool DeleteBannerById(int bannerId);
        IEnumerable<Newsletter> GetNewsletters();
        bool IsEmailExistInNewslettersCustomers(string customerEmail);
        bool AddNewsletter(string customerEmail);
        bool DeleteNewsletterById(int newsletterId);
    }
}
