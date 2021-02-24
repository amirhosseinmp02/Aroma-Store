using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aroma_Shop.Mvc.Controllers
{
    public class ManageErrors : Controller
    {
        public IActionResult Error404()
        {
            return View();
        }
    }
}
