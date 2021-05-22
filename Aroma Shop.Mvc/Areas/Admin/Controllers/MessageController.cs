using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.Message;
using Aroma_Shop.Domain.Models;
using Aroma_Shop.Domain.Models.MessageModels;
using Microsoft.AspNetCore.Authorization;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Authorize(Policy = "Writer")]
    [Area("Admin")]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        #region ShowMessages

        [HttpGet("/Admin/Messages")]
        public IActionResult Index(int pageNumber = 1, string search = null)
        {
            IEnumerable<Message> messages;
            if (!string.IsNullOrEmpty(search))
            {
                messages = _messageService.GetMessages()
                    .Where(p => p.MessageSenderName.Contains(search)
                                || p.MessageDescription.Contains(search)
                                || p.MessageSubject.Contains(search)
                                || p.MessageSenderEmail.Contains(search))
                    .OrderBy(p => p.IsRead);
                ViewBag.search = search;
            }
            else
                messages =
                    _messageService.GetMessages()
                        .OrderBy(p => p.IsRead);
            if (!messages.Any())
            {
                ViewBag.isEmpty = true;
                return View();
            }
            var page = new Paging<Message>(messages, 11, pageNumber);
            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();
            var messagesPage = page.QueryResult;
            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;

            return View(messagesPage);
        }

        #endregion

        #region MessageDetail

        [HttpGet("/Admin/Messages/{messageId}")]
        public IActionResult MessageDetail(int messageId)
        {
            var message = _messageService.GetMessage(messageId);
            var messageDetailViewModel = new MessageDetailViewModel()
            {
                Message = message,
                MessageReplyDescription = message.MessageReply?.MessageReplyDescription
            };
            _messageService.SetAsRead(message);
            return View(messageDetailViewModel);
        }

        #endregion

        #region ReplyToMessage

        [HttpPost]
        public async Task<IActionResult> ReplyToMessage(MessageDetailViewModel messageDetailViewModel)
        {
            var messageId = Convert.ToInt32(TempData["messageId"]);
            var result = await _messageService.ReplyToMessage(messageDetailViewModel.MessageReplyDescription, messageId);
            var message = _messageService.GetMessage(messageId);
            messageDetailViewModel.Message = message;
            if (result)
            {
                ViewData["SuccessMessage"] = "پیام شما با موفقیت ارسال شد.";
                ModelState.Clear();
                return View("MessageDetail", messageDetailViewModel);
            }
            ModelState.AddModelError("", "مشکلی در زمان ارسال پیام رخ داد.");
            TempData.Keep("messageId");
            return View("MessageDetail", messageDetailViewModel);
        }
        #endregion

        #region DeleteMessage

        [HttpGet("/Admin/Messages/DeleteMessage")]
        public IActionResult DeleteMessage(int messageId)
        {
            var result = _messageService.DeleteMessageById(messageId);

            if (result)
                return RedirectToAction("Index");

            return NotFound();
        }

        #endregion
    }
}
