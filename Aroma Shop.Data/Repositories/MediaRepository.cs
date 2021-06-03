﻿using System;
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
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
