using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Domain.Models;
using Aroma_Shop.Domain.Models.ProductModels;
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
                                || p.MessageSubject.Contains(search));
                ViewBag.search = search;
            }
            else
                messages = _messageService.GetMessages();
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
