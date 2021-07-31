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

        public IEnumerable<Comment> GetComments()
        {
            var comments =
                _mediaRepository.GetComments();

            return comments;
        }
        public int GetUnreadCommentsCount()
        {
            var unreadCommentsCount =
                _mediaRepository.GetUnreadCommentsCount();

            return unreadCommentsCount;
        }
        public async Task<bool> AddReplyToCommentByAdmin(int commentId, string newCommentReplyDescription)
        {
            try
            {
                var comment =
                    GetComment(commentId);

                if (comment == null)
                    return false;

                var loggedUser =
                    await _accountService.GetLoggedUser();

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

                _mediaRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> AddCommentToProduct(ProductViewModel productViewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productViewModel.CommentDescription))
                    return false;

                var user =
                    await _accountService.GetLoggedUser();

                var comment = new Comment()
                {
                    SubmitTime = DateTime.Now,
                    CommentDescription = productViewModel.CommentDescription,
                    Product = productViewModel.Product,
                    User = user
                };

                productViewModel.Product.Comments.Add(comment);

                _mediaRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> AddReplyToProductComment(ProductViewModel productViewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productViewModel.CommentDescription))
                    return false;

                var parentComment =
                    GetComment(productViewModel.ParentCommentId);
                if (parentComment == null)
                    return false;

                var loggedUser =
                    await _accountService.GetLoggedUser();

                var commentReply = new Comment()
                {
                    SubmitTime = DateTime.Now,
                    CommentDescription = productViewModel.CommentDescription,
                    Product = productViewModel.Product,
                    User = loggedUser
                };

                parentComment.Replies.Add(commentReply);

                _mediaRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public Comment GetComment(int commentId)
        {
            var comment =
                _mediaRepository.GetComment(commentId);

            return comment;
        }
        public bool UpdateComment(Comment comment)
        {
            try
            {
                _mediaRepository.UpdateComment(comment);

                _mediaRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool SetCommentAsRead(Comment comment)
        {
            try
            {
                if (comment.IsRead)
                    return true;

                comment.IsRead = true;

                _mediaRepository
                    .UpdateComment(comment);

                _mediaRepository.Save();

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
        public bool DeleteComment(Comment comment)
        {
            try
            {
                if (comment.Replies.NotNullOrEmpty())
                {
                    _mediaRepository
                        .DeleteComments(comment.Replies);
                }

                _mediaRepository.DeleteComment(comment);

                _mediaRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool DeleteCommentById(int commentId)
        {
            try
            {
                var comment =
                    GetComment(commentId);

                DeleteComment(comment);

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool ConfirmComment(int commentId)
        {
            try
            {
                var comment =
                    GetComment(commentId);

                if (comment == null)
                    return false;

                comment.IsRead = true;
                comment.IsConfirmed = true;

                _mediaRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool RejectComment(int commentId)
        {
            try
            {
                var comment =
                    GetComment(commentId);

                if (comment.Replies.Any())
                {
                    foreach (var commentReply in comment.Replies)
                    {
                        commentReply.IsConfirmed = false;
                    }
                }

                comment.IsConfirmed = false;

                _mediaRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        
        public bool AddMessage(Message message)
        {
            try
            {
                message.SubmitTime = DateTime.Now;

                _mediaRepository.AddMessage(message);
                _mediaRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
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

                _mediaRepository.DeleteMessage(message);
                _mediaRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public Message GetMessage(int messageId)
        {
            var message =
                _mediaRepository.GetMessage(messageId);

            return message;
        }
        public IEnumerable<Message> GetMessages()
        {
            var messages =
                _mediaRepository.GetMessages();

            return messages;
        }
        public int GetUnreadMessagesCount()
        {
            var getUnreadMessagesCount =
                _mediaRepository.GetUnreadMessagesCount();

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

                _mediaRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool SetMessageAsRead(Message message)
        {
            try
            {
                if (message.IsRead)
                    return true;

                message.IsRead = true;

                _mediaRepository
                    .UpdateMessage(message);

                _mediaRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public IEnumerable<Banner> GetBanners()
        {
            var banners =
                _mediaRepository
                    .GetBanners();

            return banners;
        }
        public EditBannerViewModel GetBannerForEdit(int bannerId)
        {
            var banner =
                _mediaRepository
                    .GetBanner(bannerId);

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
        public bool AddBanner(AddBannerViewModel bannerViewModel)
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
                    _fileService
                        .AddBannerImage(banner,bannerViewModel.BannerImage);

                if (!addBannerImageResult)
                    return false;

                _mediaRepository
                    .AddBanner(banner);

                _mediaRepository
                    .Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool UpdateBanner(EditBannerViewModel bannerViewModel)
        {
            try
            {
                var currentBanner =
                    _mediaRepository
                        .GetBanner(bannerViewModel.BannerId);

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
                        _fileService
                            .AddBannerImage(currentBanner, bannerViewModel.BannerImage);

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

                _mediaRepository
                    .Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool DeleteBannerById(int bannerId)
        {
            try
            {
                var banner =
                    _mediaRepository
                        .GetBanner(bannerId);

                if (banner == null)
                    return false;

                var deleteBannerImageResult =
                    _fileService
                        .DeleteBannerImage(banner.BannerImage);

                if (!deleteBannerImageResult)
                    return false;

                _mediaRepository
                    .DeleteBanner(banner);

                _mediaRepository
                    .Save();

                return true;

            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public IEnumerable<Newsletter> GetNewsletters()
        {
            var newsletters =
                _mediaRepository
                    .GetNewsletters();

            return newsletters;
        }
        public bool IsEmailExistInNewslettersCustomers(string customerEmail)
        {
            var isEmailExistInNewslettersCustomers =
                _mediaRepository
                    .IsEmailExistInNewslettersCustomers(customerEmail);

            return isEmailExistInNewslettersCustomers;
        }
        public bool AddNewsletter(string customerEmail)
        {
            try
            {
                var newsletter = new Newsletter()
                {
                    CustomerEmail = customerEmail
                };

                _mediaRepository
                    .AddNewsletter(newsletter);

                _mediaRepository
                    .Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool DeleteNewsletterById(int newsletterId)
        {
            try
            {
                _mediaRepository
                    .DeleteNewsletterById(newsletterId);

                _mediaRepository
                    .Save();

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
