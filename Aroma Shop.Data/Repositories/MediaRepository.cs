using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<Comment> GetCommentWithDetailsAsync(int commentId)
        {
            var comment =
                await _context
                    .Comments
                    .Include(p => p.ParentComment)
                    .Include(p => p.User)
                    .Include(p => p.Product)
                    .Include(p => p.Replies)
                    .ThenInclude(p => p.User)
                    .SingleOrDefaultAsync(p => p.CommentId == commentId);

            return comment;
        }
        public async Task<Comment> GetCommentAsync(int commentId)
        {
            var comment =
                await _context
                    .Comments
                    .FindAsync(commentId);

            return comment;
        }
        public void UpdateCommentAsync(Comment comment)
        {
            _context.Update(comment);
        }
        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            var comments =
                await _context
                    .Comments
                    .Include(p => p.Product)
                    .Include(p => p.User)
                    .Include(p => p.ParentComment)
                    .ThenInclude(p => p.User)
                    .ToListAsync();

            return comments;
        }
        public async Task<int> GetUnreadCommentsCountAsync()
        {
            var unreadMessagesCount =
                await _context
                    .Comments
                    .CountAsync(p => !p.IsRead);

            return unreadMessagesCount;
        }
        public void DeleteComments(IEnumerable<Comment> comments)
        {
            _context
                .RemoveRange(comments);
        }
        public void DeleteComment(Comment comment)
        {
            _context.Remove(comment);
        }
        public async void DeleteCommentById(int commentId)
        {
            var comment =
                await GetCommentAsync(commentId);

            DeleteComment(comment);
        }
        public async Task AddMessageAsync(Message message)
        {
            await _context
                .AddAsync(message);
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
        public async Task<Message> GetMessageAsync(int messageId)
        {
            var message =
                await _context
                    .Messages
                    .Include(p => p.MessageReply)
                    .SingleOrDefaultAsync(p => p.MessageId == messageId);

            return message;
        }
        public async Task<int> GetMessagesCountAsync()
        {
            var messagesCount =
                await _context
                    .Messages
                    .CountAsync();

            return messagesCount;
        }
        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            var messages =
                await _context
                    .Messages
                    .ToListAsync();

            return messages;
        }
        public async Task<int> GetUnreadMessagesCountAsync()
        {
            var unreadMessagesCount =
                await _context
                    .Messages
                    .CountAsync(p => !p.IsRead);

            return unreadMessagesCount;
        }
        public async Task<IEnumerable<Banner>> GetBannersAsync()
        {
            var banners =
                await _context
                    .Banners
                    .Include(p => p.BannerImage)
                    .ToListAsync();

            return banners;
        }
        public async Task<Banner> GetBannerAsync(int bannerId)
        {
            var banner =
                await _context
                    .Banners
                    .Include(p => p.BannerImage)
                    .SingleOrDefaultAsync(p => p.BannerId == bannerId);

            return banner;
        }
        public async Task AddBannerAsync(Banner banner)
        {
            await _context
                .AddAsync(banner);
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
        public async Task<IEnumerable<Newsletter>> GetNewslettersAsync()
        {
            var newsletters =
                await _context
                    .Newsletters
                    .ToListAsync();

            return newsletters;
        }
        public async Task<bool> IsEmailExistInNewslettersCustomersAsync(string customerEmail)
        {
            var isEmailExistInNewslettersCustomers =
                await _context
                    .Newsletters
                    .AnyAsync(p => p.CustomerEmail == customerEmail);

            return isEmailExistInNewslettersCustomers;
        }
        public async Task AddNewsletterAsync(Newsletter newsletter)
        {
            await _context
                .AddAsync(newsletter);
        }
        public async Task DeleteNewsletterByIdAsync(int newsletterId)
        {
            var newsletter =
                await _context
                    .Newsletters
                    .FindAsync(newsletterId);

            _context
                .Remove(newsletter);
        }

        public async Task SaveAsync()
        {
            await _context
                .SaveChangesAsync();
        }
    }
}
