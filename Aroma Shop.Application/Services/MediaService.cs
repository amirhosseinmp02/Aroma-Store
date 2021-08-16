using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.Banner;
using Aroma_Shop.Application.ViewModels.Message;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.MediaModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aroma_Shop.Application.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IFileService _fileService;

        public MediaService(IMediaRepository mediaRepository, IAccountService accountService,IEmailService emailService, IHttpContextAccessor accessor, IFileService fileService)
        {
            _mediaRepository = mediaRepository;
            _accountService = accountService;
            _emailService = emailService;
            _accessor = accessor;
            _fileService = fileService;
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            var comments =
                await _mediaRepository
                    .GetCommentsAsync();

            return comments;
        }
        public async Task<int> GetUnreadCommentsCountAsync()
        {
            var unreadCommentsCount =
                await _mediaRepository
                    .GetUnreadCommentsCountAsync();

            return unreadCommentsCount;
        }
        public async Task<bool> AddReplyToCommentByAdminAsync(int commentId, string newCommentReplyDescription)
        {
            try
            {
                var comment =
                    await GetCommentAsync(commentId);

                if (comment == null)
                    return false;

                var loggedUser =
                    await _accountService
                        .GetLoggedUserAsync();

                var commentReply = new Comment()
                {
                    SubmitTime = DateTime.Now,
                    CommentDescription = newCommentReplyDescription,
                    Product = comment.Product,
                    User = loggedUser,
                    IsRead = true,
                    IsConfirmed = true,
                    IsAdminReplied = true
                };

                comment.Replies.Add(commentReply);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> AddCommentToProductAsync(ProductViewModel productViewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productViewModel.CommentDescription))
                    return false;

                var user =
                    await _accountService
                        .GetLoggedUserAsync();

                var comment = new Comment()
                {
                    SubmitTime = DateTime.Now,
                    CommentDescription = productViewModel.CommentDescription,
                    Product = productViewModel.Product,
                    User = user
                };

                productViewModel.Product.Comments.Add(comment);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> AddReplyToProductCommentAsync(ProductViewModel productViewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productViewModel.CommentDescription))
                    return false;

                var parentComment =
                    await GetCommentAsync(productViewModel.ParentCommentId);

                if (parentComment == null)
                    return false;

                var loggedUser =
                    await _accountService
                        .GetLoggedUserAsync();

                var commentReply = new Comment()
                {
                    SubmitTime = DateTime.Now,
                    CommentDescription = productViewModel.CommentDescription,
                    Product = productViewModel.Product,
                    User = loggedUser
                };

                parentComment.Replies.Add(commentReply);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<Comment> GetCommentAsync(int commentId)
        {
            var comment =
                await _mediaRepository
                    .GetCommentWithDetailsAsync(commentId);

            return comment;
        }
        public async Task<bool> UpdateCommentAsync(Comment comment)
        {
            try
            {
                _mediaRepository
                    .UpdateCommentAsync(comment);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> SetCommentAsReadAsync(Comment comment)
        {
            try
            {
                if (comment.IsRead)
                    return true;

                comment.IsRead = true;

                _mediaRepository
                    .UpdateCommentAsync(comment);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool DeleteCommentsByParents(IEnumerable<Comment> parentsComments)
        {
            try
            {
                foreach (var comment in parentsComments) 
                {
                    if (comment.Replies.NotNullOrEmpty())
                    {
                        _mediaRepository
                            .DeleteComments(comment.Replies);
                    }
                }

                _mediaRepository
                    .DeleteComments(parentsComments);

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> DeleteCommentAsync(Comment comment)
        {
            try
            {
                if (comment.Replies.NotNullOrEmpty())
                {
                    _mediaRepository
                        .DeleteComments(comment.Replies);
                }

                _mediaRepository
                    .DeleteComment(comment);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> DeleteCommentByIdAsync(int commentId)
        {
            try
            {
                var comment =
                    await GetCommentAsync(commentId);

                await DeleteCommentAsync(comment);

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> ConfirmCommentAsync(int commentId)
        {
            try
            {
                var comment =
                    await GetCommentAsync(commentId);

                if (comment == null)
                    return false;

                comment.IsRead = true;
                comment.IsConfirmed = true;

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> RejectCommentAsync(int commentId)
        {
            try
            {
                var comment =
                    await GetCommentAsync(commentId);

                if (comment.Replies.Any())
                {
                    foreach (var commentReply in comment.Replies)
                    {
                        commentReply.IsConfirmed = false;
                    }
                }

                comment
                    .IsConfirmed = false;

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        
        public async Task<bool> AddMessageAsync(Message message)
        {
            try
            {
                message.SubmitTime = 
                    DateTime.Now;

                await _mediaRepository
                    .AddMessageAsync(message);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> DeleteMessageByIdAsync(int messageId)
        {
            try
            {
                var message = 
                    await GetMessageAsync(messageId);

                if (message == null)
                    return false;

                _mediaRepository
                    .DeleteMessage(message);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<int> GetMessagesCountAsync()
        {
            var messagesCount =
                await _mediaRepository
                    .GetMessagesCountAsync();

            return messagesCount;
        }
        public async Task<Message> GetMessageAsync(int messageId)
        {
            var message =
                await _mediaRepository
                    .GetMessageAsync(messageId);

            return message;
        }
        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            var messages =
                await _mediaRepository
                    .GetMessagesAsync();

            return messages;
        }
        public async Task<int> GetUnreadMessagesCountAsync()
        {
            var getUnreadMessagesCount =
                await _mediaRepository
                    .GetUnreadMessagesCountAsync();

            return getUnreadMessagesCount;
        }
        public async Task<bool> ReplyToMessageAsync(string messageReplyDescription, int messageId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(messageReplyDescription))
                    return false;

                var message =
                    await GetMessageAsync(messageId);

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

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> SetMessageAsReadAsync(Message message)
        {
            try
            {
                if (message.IsRead)
                    return true;

                message.IsRead = true;

                _mediaRepository
                    .UpdateMessage(message);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<IEnumerable<Banner>> GetBannersAsync()
        {
            var banners =
                await _mediaRepository
                    .GetBannersAsync();

            return banners;
        }
        public async Task<EditBannerViewModel> GetBannerForEditAsync(int bannerId)
        {
            var banner =
                await _mediaRepository
                    .GetBannerAsync(bannerId);

            if (banner == null)
                return null;

            var bannerViewModel = new EditBannerViewModel()
            {
                BannerId = bannerId,
                BannerCurrentImagePath = banner.BannerImage.ImagePath,
                BannerLink = banner.BannerLink,
                BannerTitle = banner.BannerTitle,
                BannerDescription = banner.BannerDescription,
                IsPrimaryBanner = banner.IsPrimaryBanner
            };

            return bannerViewModel;
        }
        public async Task<bool> AddBannerAsync(AddBannerViewModel bannerViewModel)
        {
            try
            {
                var banner = new Banner()
                {
                    BannerTitle = bannerViewModel.BannerTitle,
                    BannerDescription = bannerViewModel.BannerDescription,
                    BannerLink = bannerViewModel.BannerLink,
                    IsPrimaryBanner = bannerViewModel.IsPrimaryBanner
                };

                var addBannerImageResult =
                    await _fileService
                        .AddBannerImageAsync(banner,bannerViewModel.BannerImage);

                if (!addBannerImageResult)
                    return false;

                await _mediaRepository
                    .AddBannerAsync(banner);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> UpdateBannerAsync(EditBannerViewModel bannerViewModel)
        {
            try
            {
                var currentBanner =
                    await _mediaRepository
                        .GetBannerAsync(bannerViewModel.BannerId);

                if (currentBanner == null)
                    return false;

                if (bannerViewModel.BannerImage != null)
                {
                    var deleteBannerImageResult =
                        _fileService
                            .DeleteBannerImage(currentBanner.BannerImage);

                    if (!deleteBannerImageResult)
                        return false;

                    var addBannerImageResult =
                        await _fileService
                            .AddBannerImageAsync(currentBanner, bannerViewModel.BannerImage);

                    if (!addBannerImageResult)
                        return false;
                }

                currentBanner
                        .BannerTitle =
                    bannerViewModel
                        .BannerTitle;

                currentBanner
                        .BannerLink =
                    bannerViewModel
                        .BannerLink;

                currentBanner
                        .BannerDescription =
                    bannerViewModel
                        .BannerDescription;

                currentBanner
                        .IsPrimaryBanner =
                    bannerViewModel
                        .IsPrimaryBanner;

                _mediaRepository
                    .UpdateBanner(currentBanner);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> DeleteBannerByIdAsync(int bannerId)
        {
            try
            {
                var banner =
                    await _mediaRepository
                        .GetBannerAsync(bannerId);

                if (banner == null)
                    return false;

                var deleteBannerImageResult =
                    _fileService
                        .DeleteBannerImage(banner.BannerImage);

                if (!deleteBannerImageResult)
                    return false;

                _mediaRepository
                    .DeleteBanner(banner);

                await _mediaRepository
                    .SaveAsync();

                return true;

            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<IEnumerable<Newsletter>> GetNewslettersAsync()
        {
            var newsletters =
                await _mediaRepository
                    .GetNewslettersAsync();

            return newsletters;
        }
        public async Task<bool> IsEmailExistInNewslettersCustomersAsync(string customerEmail)
        {
            var isEmailExistInNewslettersCustomers =
                await _mediaRepository
                    .IsEmailExistInNewslettersCustomersAsync(customerEmail);

            return isEmailExistInNewslettersCustomers;
        }
        public async Task<bool> AddNewsletterAsync(string customerEmail)
        {
            try
            {
                var newsletter = new Newsletter()
                {
                    CustomerEmail = customerEmail
                };

                await _mediaRepository
                    .AddNewsletterAsync(newsletter);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> DeleteNewsletterByIdAsync(int newsletterId)
        {
            try
            {
                await _mediaRepository
                    .DeleteNewsletterByIdAsync(newsletterId);

                await _mediaRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
    }
}
