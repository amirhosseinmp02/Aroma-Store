using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.MediaModels;
using Microsoft.EntityFrameworkCore;

namespace Aroma_Shop.Data.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly AppDbContext _context;

        public MediaRepository(AppDbContext context)
        {
            _context = context;
        }

        public Comment GetComment(int commentId)
        {
            var comment =
                _context.Comments
                    .Include(p => p.ParentComment)
                    .Include(p => p.User)
                    .Include(p => p.Product)
                    .Include(p => p.Replies)
                    .ThenInclude(p => p.User)
                    .SingleOrDefault(p => p.CommentId == commentId);

            return comment;
        }
        public void UpdateComment(Comment comment)
        {
            _context.Update(comment);
        }
        public IEnumerable<Comment> GetComments()
        {
            var comments =
                _context.Comments
                    .Include(p => p.Product)
                    .Include(p => p.User)
                    .Include(p => p.ParentComment)
                    .ThenInclude(p => p.User);

            return comments;
        }
        public int GetUnreadCommentsCount()
        {
            var unreadMessagesCount =
                _context.Comments.Count(p => !p.IsRead);

            return unreadMessagesCount;
        }
        public void DeleteComment(Comment comment)
        {
            _context.Remove(comment);
        }
        public void DeleteCommentById(int commentId)
        {
            var comment =
                GetComment(commentId);

            _context.Remove(comment);
        }
        public void AddMessage(Message message)
        {
            _context.Add(message);
        }
        public void UpdateMessage(Message message)
        {
            _context
                .Update(message);
        }
        public void DeleteMessage(Message message)
        {
            _context.Remove(message);
        }
        public Message GetMessage(int messageId)
        {
            var message =
                _context.Messages
                    .Include(p => p.MessageReply)
                    .SingleOrDefault(p => p.MessageId == messageId);

            return message;
        }
        public IEnumerable<Message> GetMessages()
        {
            var messages =
                _context.Messages;

            return messages;
        }
        public int GetUnreadMessagesCount()
        {
            var unreadMessagesCount =
                _context.Messages.Count(p => !p.IsRead);

            return unreadMessagesCount;
        }
        public IEnumerable<Banner> GetBanners()
        {
            var banners =
                _context
                    .Banners
                    .Include(p => p.BannerImage);

            return banners;
        }
        public Banner GetBanner(int bannerId)
        {
            var banner =
                _context
                    .Banners
                    .Include(p => p.BannerImage)
                    .SingleOrDefault(p => p.BannerId == bannerId);

            return banner;
        }
        public void AddBanner(Banner banner)
        {
            _context
                .Add(banner);
        }
        public void UpdateBanner(Banner banner)
        {
            _context
                .Update(banner);
        }
        public void DeleteBanner(Banner banner)
        {
            _context
                .Remove(banner);
        }
        public IEnumerable<Newsletter> GetNewsletters()
        {
            var newsletters =
                _context
                    .Newsletters;

            return newsletters;
        }
        public bool IsEmailExistInNewslettersCustomers(string customerEmail)
        {
            var isEmailExistInNewslettersCustomers =
                _context
                    .Newsletters
                    .Any(p => p.CustomerEmail == customerEmail);

            return isEmailExistInNewslettersCustomers;
        }
        public void AddNewsletter(Newsletter newsletter)
        {
            _context
                .Add(newsletter);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
