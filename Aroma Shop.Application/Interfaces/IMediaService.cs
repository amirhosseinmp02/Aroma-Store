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
        public Task<IEnumerable<Comment>> GetCommentsAsync();
        public Task<int> GetUnreadCommentsCountAsync();
        public Task<bool> AddReplyToCommentByAdminAsync(int commentId, string newCommentReplyDescription);
        public Task<bool> AddCommentToProductAsync(ProductViewModel productViewModel);
        public Task<bool> AddReplyToProductCommentAsync(ProductViewModel productViewModel);
        public Task<Comment> GetCommentAsync(int commentId);
        public Task<bool> UpdateCommentAsync(Comment comment);
        public Task<bool> SetCommentAsReadAsync(Comment comment);
        public bool DeleteCommentsByParents(IEnumerable<Comment> parentsComments);
        public Task<bool> DeleteCommentAsync(Comment comment);
        public Task<bool> DeleteCommentByIdAsync(int commentId);
        public Task<bool> ConfirmCommentAsync(int commentId);
        public Task<bool> RejectCommentAsync(int commentId);
        public Task<bool> AddMessageAsync(Message message);
        public Task<bool> DeleteMessageByIdAsync(int messageId);
        public Task<int> GetMessagesCountAsync();
        public Task<Message> GetMessageAsync(int messageId);
        public Task<IEnumerable<Message>> GetMessagesAsync();
        public Task<int> GetUnreadMessagesCountAsync();
        public Task<bool> ReplyToMessageAsync(string messageReplyDescription, int messageId);
        public Task<bool> SetMessageAsReadAsync(Message message);
        public Task<IEnumerable<Banner>> GetBannersAsync();
        public Task<EditBannerViewModel> GetBannerForEditAsync(int bannerId);
        public Task<bool> AddBannerAsync(AddBannerViewModel bannerViewModel);
        public Task<bool> UpdateBannerAsync(EditBannerViewModel bannerViewModel);
        public Task<bool> DeleteBannerByIdAsync(int bannerId);
        public Task<IEnumerable<Newsletter>> GetNewslettersAsync();
        public Task<bool> IsEmailExistInNewslettersCustomersAsync(string customerEmail);
        public Task<bool> AddNewsletterAsync(string customerEmail);
        public Task<bool> DeleteNewsletterByIdAsync(int newsletterId);
    }
}
