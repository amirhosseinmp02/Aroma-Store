using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Authorize(Policy = "Writer")]
    [Area("Admin")]
    public class UserController : Controller
    {
        #region ShowUsers

        [HttpGet("/Admin/Users")]
        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
