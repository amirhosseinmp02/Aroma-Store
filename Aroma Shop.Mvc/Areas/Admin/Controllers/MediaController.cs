using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.Banner;
using Aroma_Shop.Application.ViewModels.Message;
using Aroma_Shop.Application.ViewModels.Newsletter;
using Aroma_Shop.Domain.Models.MediaModels;
using Microsoft.AspNetCore.Authorization;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Authorize(Policy = "Writer")]
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
        public async Task<IActionResult> Messages(int pageNumber = 1, string search = null)
        {
            var messages =
                await _mediaService
                    .GetMessagesAsync();

            if (!string.IsNullOrEmpty(search))
            {
                messages = 
                    messages
                        .Where(p => p.MessageSenderName.Contains(search)
                                               || p.MessageDescription.Contains(search)
                                               || p.MessageSubject.Contains(search)
                                               || p.MessageSenderEmail.Contains(search))
                    .OrderBy(p => p.IsRead);

                ViewData["search"]

                ViewBag.search = search;
            }
            else
                messages =
                    messages
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
        public async Task<IActionResult> MessageDetails(int messageId)
        {
            var message =
                await _mediaService.
                    GetMessageAsync(messageId);

            var model = new MessageDetailViewModel()
            {
                Message = message,
                MessageReplyDescription = message.MessageReply?.MessageReplyDescription,
                MessageSubmitTime = message.SubmitTime
            };

            await _mediaService
                .SetMessageAsReadAsync(message);

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
                    .ReplyToMessageAsync(model.MessageReplyDescription, messageId);

            var message =
                await _mediaService
                    .GetMessageAsync(messageId);

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
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var result = 
                await _mediaService
                    .DeleteMessageByIdAsync(messageId);

            if (result)
                return RedirectToAction("Messages");

            return NotFound();
        }

        #endregion

        #region ShowComments

        [HttpGet("/Admin/Comments")]
        public async Task<IActionResult> Comments(int pageNumber = 1, string search = null)
        {
            var comments =
                await _mediaService
                    .GetCommentsAsync();

            if (!string.IsNullOrEmpty(search))
            {
                comments =
                    comments.Where(p =>
                    p.CommentDescription.Contains(search) ||
                    p.User.UserName.Contains(search) ||
                    p.Product.ProductName.Contains(search))
                    .OrderBy(p => p.IsRead);

                ViewBag.search = search;
            }
            else
                comments =
                    comments
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
        public async Task<IActionResult> CommentDetails(int commentId)
        {
            var comment =
                await _mediaService
                    .GetCommentAsync(commentId);

            if (comment == null)
                return NotFound();

            await _mediaService
                .SetCommentAsReadAsync(comment);

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
                await _mediaService
                    .AddReplyToCommentByAdminAsync(commentId, NewCommentReplyDescription);

            if (result)
            {
                ViewData["SuccessMessageForAddCommentReply"] = "پاسخ شما با موفقیت ثبت شد";

                var comment =
                    await _mediaService.GetCommentAsync(commentId);

                return View("CommentDetails", comment);
            }

            return NotFound();
        }

        #endregion

        #region EditComment

        [HttpPost("/Admin/Comments/EditComment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(string NewCommentDescription)
        {
            var commentId =
                Convert.ToInt32(TempData["commentId"]);

            var comment =
                await _mediaService
                    .GetCommentAsync(commentId);

            comment.CommentDescription = NewCommentDescription;

            var result =
                await _mediaService
                    .UpdateCommentAsync(comment);

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
        public async Task<IActionResult> ConfirmComment(int commentId, string returnUrl)
        {
            var result =
                await _mediaService
                    .ConfirmCommentAsync(commentId);

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
        public async Task<IActionResult> RejectComment(int commentId, string returnUrl)
        {
            var result =
                await _mediaService
                    .RejectCommentAsync(commentId);

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
        public async Task<IActionResult> DeleteComment(int commentId, string returnUrl = null)
        {
            var result =
                await _mediaService
                    .DeleteCommentByIdAsync(commentId);

            if (result)
            {
                if (string.IsNullOrEmpty(returnUrl))
                    returnUrl = Url.Action("Comments");

                return Redirect(returnUrl);
            }

            return NotFound();
        }

        #endregion

        #region ShowBanners

        [HttpGet("/Admin/Banners")]
        public async Task<IActionResult> Banners(int pageNumber = 1, string search = null)
        {
            var banners =
                await _mediaService
                    .GetBannersAsync();

            if (!string.IsNullOrEmpty(search))
            {
                banners =
                    banners
                        .Where(p =>
                        p.BannerTitle.Contains(search) ||
                        p.BannerDescription.Contains(search) ||
                        p.BannerId.ToString() == search);

                ViewBag.search = search;
            }

            if (!banners.Any())
            {
                ViewBag.isEmpty = true;

                return View();
            }

            var page =
                new Paging<Banner>(banners, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();
                
            var bannersPage =
                page.QueryResult;

            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;

            return View(bannersPage);
        }

        #endregion

        #region AddBaner

        [HttpGet("/Admin/Banners/AddBanner")]
        public IActionResult AddBanner()
        {
            return View();
        }

        [HttpPost("/Admin/Banners/AddBanner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBanner(AddBannerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _mediaService
                        .AddBannerAsync(model);

                if (result)
                    return RedirectToAction("Banners");

                ModelState.AddModelError("","مشکلی در زمان افزودن بنر رخ داد");
            }

            return View(model);
        }

        #endregion

        #region EditBanner

        [HttpGet("/Admin/Banners/EditBanner")]
        public async Task<IActionResult> EditBanner(int bannerId)
        {
            var bannerViewModel =
                await _mediaService
                    .GetBannerForEditAsync(bannerId);

            if (bannerViewModel == null)
                return NotFound();

            return View(bannerViewModel);
        }

        [HttpPost("/Admin/Banners/EditBanner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBanner(EditBannerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _mediaService
                        .UpdateBannerAsync(model);

                if (result)
                    return RedirectToAction("Banners");

                ModelState.AddModelError("","مشکلی در زمان ویرایش بنر رخ داد");
            }

            return View(model);
        }

        #endregion

        #region DeleteBanner

        [HttpGet("/Admin/Banners/DeleteBanner")]
        public async Task<IActionResult> DeleteBanner(int bannerId)
        {
            var result =
                await _mediaService
                    .DeleteBannerByIdAsync(bannerId);

            if (result)
                return RedirectToAction("Banners");

            return NotFound();
        }

        #endregion

        #region ShowNewsletters

        [HttpGet("/Admin/Newsletters")]
        public async Task<IActionResult> Newsletters(int pageNumber = 1, string search = null)
        {
            var newsletters =
                await _mediaService
                    .GetNewslettersAsync();

            if (!string.IsNullOrEmpty(search))
            {
                newsletters =
                    newsletters
                        .Where(p =>
                        p.CustomerEmail.Contains(search) ||
                        p.NewsletterId.ToString() == search);

                ViewBag.search = search;
            }

            if (!newsletters.Any())
            {
                ViewBag.isEmpty = true;

                return View();
            }

            var page =
                new Paging<Newsletter>(newsletters, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var newslettersPage =   
                page.QueryResult;

            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;

            return View(newslettersPage);
        }

        #endregion

        #region AddNewsletter

        [HttpGet("/Admin/Newsletters/AddNewsletter")]
        public IActionResult AddNewsletter()
        {
            return View();
        }

        [HttpPost("/Admin/Newsletters/AddNewsletter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewsletter(AddNewsletterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _mediaService
                        .AddNewsletterAsync(model.CustomerEmail);

                if (result)
                    return RedirectToAction("Newsletters");

                ModelState.AddModelError("","مشکلی در زمان ثبت ایمیل رخ داد");
            }

            return View(model);
        }

        #endregion

        #region DeleteNewsletter

        [HttpGet("/Admin/Newsletters/DeleteNewsletter")]
        public async Task<IActionResult> DeleteNewsletter(int newsletterId)
        {
            var result =
                await _mediaService
                    .DeleteNewsletterByIdAsync(newsletterId);

            if (result)
                return RedirectToAction("Newsletters");

            return NotFound();
        }

        #endregion

        #region CheckNewNewsletterCustomerEmailExist

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsNewsletterEmailExist(string customerEmail)
        {
            var isNewsletterEmailExist =
                await _mediaService
                    .IsEmailExistInNewslettersCustomersAsync(customerEmail);

            if (!isNewsletterEmailExist)
                return new JsonResult(true);

            return new JsonResult("ایمیل قبلا ثبت شده");
        }

        #endregion
    }
}
