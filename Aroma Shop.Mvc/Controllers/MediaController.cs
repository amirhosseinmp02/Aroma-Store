using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.Home;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Models.MediaModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aroma_Shop.Mvc.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public MediaController(IMediaService mediaService, IProductService productService, IConfiguration configuration)
        {
            _mediaService = mediaService;
            _productService = productService;
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        #region ContactUs

        [HttpGet("/Contact-Us")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost("/Contact-Us")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(Message model)
        {
            string recaptchaResponse =
                Request.Form["g-recaptcha-response"];

            var url =
                "https://www.google.com/recaptcha/api/siteverify";

            var response =
                await _httpClient
                    .PostAsync($"{url}?secret={_configuration["reCAPTCHA:SecretKey"]}&response={recaptchaResponse}",
                        new StringContent(""));

            var responseString =
                await response.Content.ReadAsStringAsync();

            dynamic jsonResponse =
                JObject.Parse(responseString);

            if (jsonResponse.success != true)
                ModelState
                    .AddModelError("", "مشکلی در زمان تایید گوگل کپچا رخ داد ، لطفا بعدا تلاش کنید");

            if (ModelState.IsValid)
            {
                var result =
                    await _mediaService
                        .AddMessageAsync(model);

                if (result)
                {
                    ViewData["SuccessMessage"] = "پیام شما با موفقیت ارسال شد.";

                    ModelState.Clear();

                    return View();
                }

                ModelState.AddModelError("", "مشکلی در زمان ارسال پیام رخ داد.");
            }

            return View(model);
        }

        #endregion

        #region AddCommentToProduct

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCommentToProduct(ProductViewModel model)
        {
            string recaptchaResponse =
                Request.Form["g-recaptcha-response"];

            var url =
                "https://www.google.com/recaptcha/api/siteverify";

            var response =
                await _httpClient
                    .PostAsync($"{url}?secret={_configuration["reCAPTCHA:SecretKey"]}&response={recaptchaResponse}",
                        new StringContent(""));

            var responseString =
                await response.Content.ReadAsStringAsync();

            dynamic jsonResponse =
                JObject.Parse(responseString);

            if (jsonResponse.success != true)
                ModelState
                    .AddModelError("", "مشکلی در زمان تایید گوگل کپچا رخ داد ، لطفا بعدا تلاش کنید");

            if (ModelState.IsValid)
            {
                var productId =
                    Convert.ToInt32(TempData["productId"]);

                var product =
                    await _productService
                        .GetProductWithDetailsAsync(productId);

                product.Comments = 
                    product.Comments;

                model.Product = 
                    product;

                bool result;

                if (model.ParentCommentId == 0)
                    result = await _mediaService
                        .AddCommentToProductAsync(model);
                else
                    result = await _mediaService
                        .AddReplyToProductCommentAsync(model);

                product.Comments =
                    product.Comments
                        .Where(p => p.IsConfirmed && p.ParentComment == null)
                        .ToList();

                if (result)
                {
                    model.CommentDescription = null;

                    model.ParentCommentId = 0;

                    ViewData["SuccessMessage"] = "دیدگاه / پاسخ شما با موفقیت ثبت و بعد از تایید ادمین نمایش داده خواهد شد";

                    ModelState.Clear();

                    return View("/Views/Product/ProductDetails.cshtml", model);
                }

                ModelState.AddModelError("", "مشکلی در زمان ثبت دیدگاه / پاسخ رخ داد.");
            }

            return View("/Views/Product/ProductDetails.cshtml", model);
        }

        #endregion

        #region AddNewsletter

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewsletter(string customerEmail)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(customerEmail))
                    return Content("EmptyError");

                if (!customerEmail.IsValidEmailAddress())
                    return Content("InvalidEmail");

                if (customerEmail.Length >= 256)
                    return Content("MoreThan256Character");

                var isEmailExistInNewslettersCustomers =
                    await _mediaService
                        .IsEmailExistInNewslettersCustomersAsync(customerEmail);

                if (isEmailExistInNewslettersCustomers)
                    return Content("CustomerEmailExist");

                var result =
                    await _mediaService
                        .AddNewsletterAsync(customerEmail);

                if (result)
                    return Content("Successful");

            }

            return Content("Failed");
        }

        #endregion
    }
}
