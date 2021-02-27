﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Aroma_Shop.Application.Utilites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Aroma_Shop.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _accessor;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<CustomIdentityUser> userManager, SignInManager<CustomIdentityUser> signInManager, LinkGenerator linkGenerator, IHttpContextAccessor accessor, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _linkGenerator = linkGenerator;
            _accessor = accessor;
            _emailService = emailService;
        }

        public async Task<IdentityResult> CreateUser(CustomIdentityUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public bool SendEmailConfirmation(CustomIdentityUser user, string returnController, string returnAction)
        {
            try
            {
                var emailConfirmationToken = 
                    _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = _linkGenerator.GetUriByAction(_accessor.HttpContext, returnAction, returnController,
                    new {userName = user.UserName, token = emailConfirmationToken}
                    , _accessor.HttpContext.Request.Scheme);
                var emailMessage = 
                    ViewToStringRenderer
                        .RenderViewToStringAsync(_accessor.HttpContext.RequestServices
                            , $"~/Views/Emails/MyEmailTemplate.cshtml", url);
                _emailService.SendEmailAsync(user.Email, "تأیید ایمیل", emailMessage.ToString(), true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public JsonResult IsUserNameExist(string userName)
        {
            var user =  _userManager.FindByNameAsync(userName);
            if (user == null) 
                return new JsonResult(true);
            return new JsonResult("امکان استفاده از این نام کاربری وجود ندارد");
        }

        public JsonResult IsEmailExist(string email)
        {
            var emaill = _userManager.FindByNameAsync(email);
            if (emaill == null)
                return new JsonResult(true);
            return new JsonResult("امکان استفاده از این ایمیل وجود ندارد");
        }

        public bool IsUserSignedIn(ClaimsPrincipal user)
        {
            if (_signInManager.IsSignedIn(user))
                return true;
            return false;
        }
    }
}
