using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Mvc.Models.CustomMiddleWares
{
    public class VisitorCounterMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public VisitorCounterMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context, IVisitorService visitorService)
        {
            string visitorId = context.Request.Cookies["VisitorId"];

            if (visitorId == null)
            {
                var remoteIpAddress =
                    context.Connection.RemoteIpAddress.ToString();

                await visitorService
                    .AddOrUpdateVisitorAsync(remoteIpAddress);

                context.Response.Cookies.Append("VisitorId", Guid.NewGuid().ToString(), new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                });
            }

            await _requestDelegate(context);
        }   
    }
}
