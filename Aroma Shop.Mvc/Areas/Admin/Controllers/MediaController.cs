using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.Message;
using Aroma_Shop.Domain.Models.MediaModels;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        #region ShowMessages

        [HttpGet("/Admin/Messages")]
        public IActionResult Messages(int pageNumber = 1, string search = null)
        {
            IEnumerable<Message> messages;

            if (!string.IsNullOrEmpty(search))
            {
                messages = _mediaService.GetMessages()
                    .Where(p => p.MessageSenderName.Contains(search)
                                || p.MessageDescription.Contains(search)
                                || p.MessageSubject.Contains(search)
                                || p.MessageSenderEmail.Contains(search))
                    .OrderBy(p => p.IsRead);

                ViewBag.search = search;
            }
            else
                messages =
                    _mediaService.GetMessages()
                        .OrderBy(p => p.IsRead);

            if (!messages.Any())
            {
                ViewBag.isEmpty = true;

                return View();
            }

            var page =
                new Paging<Message>(messages, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var messagesPage =
                page.QueryResult;

            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;

            return View(messagesPage);
        }

        #endregion

        #region MessageDetails

        [HttpGet("/Admin/Messages/{messageId}")]
        public IActionResult MessageDetails(int messageId)
        {
            var message =
                _mediaService.GetMessage(messageId);

            var model = new MessageDetailViewModel()
            {
                Message = message,
                MessageReplyDescription = message.MessageReply?.MessageReplyDescription,
                MessageSubmitTime = message.SubmitTime
            };

            _mediaService.SetMessageAsRead(message);

            return View(model);
        }

        #endregion

        #region ReplyToMessage

        [HttpPost("/Admin/Messages/ReplyToMessage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyToMessage(MessageDetailViewModel model)
        {
            var messageId =
                Convert.ToInt32(TempData["messageId"]);

            var result =
                await _mediaService
                    .ReplyToMessage(model.MessageReplyDescription, messageId);

            var message =
                _mediaService.GetMessage(messageId);

            model.Message = message;

            if (result)
            {
                ViewData["SuccessMessage"] = "پیام شما با موفقیت ارسال شد.";

                ModelState.Clear();

                model.MessageSubmitTime = 
                    message.MessageReply.MessageReplySubmitTime;

                return View("MessageDetails", model);
            }
            ModelState.AddModelError("", "مشکلی در زمان ارسال پیام رخ داد.");

            TempData.Keep("messageId");

            return View("MessageDetails", model);
        }
        #endregion

        #region DeleteMessage

        [HttpGet("/Admin/Messages/DeleteMessage")]
        public IActionResult DeleteMessage(int messageId)
        {
            var result = 
                _mediaService.DeleteMessageById(messageId);

            if (result)
                return RedirectToAction("Messages");

            return NotFound();
        }

        #endregion

        #region ShowComments

        [HttpGet("/Admin/Comments")]
        public IActionResult Comments(int pageNumber = 1, string search = null)
        {
            IEnumerable<Comment> comments;

            if (!string.IsNullOrEmpty(search))
            {
                comments =
                    _mediaService.GetComments().Where(p =>
                    p.CommentDescription.Contains(search) ||
                    p.User.UserName.Contains(search) ||
                    p.Product.ProductName.Contains(search))
                    .OrderBy(p => p.IsRead);

                ViewBag.search = search;
            }
            else
                comments =
                    _mediaService.GetComments()
                    .OrderBy(p => p.IsRead);

            if (!comments.Any())
            {
                ViewBag.isEmpty = true;

                return View();
            }

            var page =
                new Paging<Comment>(comments, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var commentsPage =
                page.QueryResult;

            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;

            return View(commentsPage);
        }

        #endregion

        #region CommentDetails

        [HttpGet("/Admin/Comments/{commentId}")]
        public IActionResult CommentDetails(int commentId)
        {
            var comment =
                _mediaService.GetComment(commentId);

            if (comment == null)
                return NotFound();

            _mediaService.SetCommentAsRead(comment);

            return View(comment);
        }

        #endregion

        #region ReplyToCommentByAdmin

        [HttpPost("/Admin/Comments/ReplyToCommentByAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReplyToCommentByAdmin(string NewCommentReplyDescription)
        {
            var commentId =
                Convert.ToInt32(TempData["commentId"]);

            var result =
                await _mediaService.AddReplyToCommentByAdmin(commentId, NewCommentReplyDescription);

            if (result)
            {
                ViewData["SuccessMessageForAddCommentReply"] = "پاسخ شما با موفقیت ثبت شد";

                var comment =
                    _mediaService.GetComment(commentId);

                return View("CommentDetails", comment);
            }

            return NotFound();
        }

        #endregion

        #region EditComment

        [HttpPost("/Admin/Comments/EditComment")]
        [ValidateAntiForgeryToken]
        public IActionResult EditComment(string NewCommentDescription)
        {
            var commentId =
                Convert.ToInt32(TempData["commentId"]);

            var comment =
                _mediaService.GetComment(commentId);

            comment.CommentDescription = NewCommentDescription;

            var result =
                _mediaService.UpdateComment(comment);

            if (result)
            {
                ViewData["SuccessMessageForEditComment"] = "دیدگاه مورد نظر با موفقیت ویرایش شد";

                return View("CommentDetails", comment);
            }

            return NotFound();
        }

        #endregion

        #region ConfirmComment

        [HttpGet("/Admin/Comments/ConfirmComment")]
        public IActionResult ConfirmComment(int commentId, string returnUrl)
        {
            var result =
                _mediaService.ConfirmComment(commentId);

            if (result)
            {
                if (string.IsNullOrEmpty(returnUrl))
                    returnUrl = Url.Action("Comments");

                return Redirect(returnUrl);
            }

            return NotFound();
        }

        #endregion

        #region RejectComment

        [HttpGet("/Admin/Comments/RejectComment")]
        public IActionResult RejectComment(int commentId, string returnUrl)
        {
            var result =
                _mediaService.RejectComment(commentId);

            if (result)
            {
                if (string.IsNullOrEmpty(returnUrl))
                    returnUrl = Url.Action("Comments");

                return Redirect(returnUrl);
            }

            return NotFound();
        }

        #endregion

        #region DeleteComment

        [HttpGet("/Admin/Comments/DeleteComment")]
        public IActionResult DeleteComment(int commentId, string returnUrl = null)
        {
            var result =
                _mediaService.DeleteCommentById(commentId);

            if (result)
            {
                if (string.IsNullOrEmpty(returnUrl))
                    returnUrl = Url.Action("Comments");

                return Redirect(returnUrl);
            }

            return NotFound();
        }

        #endregion

        #region AddBaner

        [HttpGet("/Admin/Banners/AddBanner")]
        public IActionResult AddBanner()
        {
            return View();
        }

        #endregion
    }
}
