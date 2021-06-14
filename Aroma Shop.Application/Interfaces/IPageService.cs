using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Application.ViewModels.Page;
using Aroma_Shop.Domain.Models.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IPageService
    {
        Page GetPage(int pageId);
        Page GetPageByPathAddress(string pagePathAddress);
        IEnumerable<Page> GetPages();
        JsonResult IsPagePathAddressExistForAddJsonResult(string pagePathAddress);
        JsonResult IsPagePathAddressExistForEditJsonResult(string newPagePathAddress, int pageId);
        PageCreateUpdateResult CreatePage(AddPageViewModel pageViewModel);
        PageCreateUpdateResult UpdatePage(EditPageViewModel pageViewModel);
        bool DeletePageById(int pageId);
    }
}
