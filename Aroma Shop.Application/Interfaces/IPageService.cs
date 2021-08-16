using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.ViewModels.Page;
using Aroma_Shop.Domain.Models.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IPageService
    {
        Task<Page> GetPageAsync(int pageId);
        Task<Page> GetPageByPathAddressAsync(string pagePathAddress);
        Task<IEnumerable<Page>> GetPagesAsync();
        Task<JsonResult> IsPagePathAddressExistForAddJsonResultAsync(string pagePathAddress);
        Task<JsonResult> IsPagePathAddressExistForEditJsonResultAsync(string newPagePathAddress, int pageId);
        Task<PageCreateUpdateResult> CreatePageAsync(AddPageViewModel pageViewModel);
        Task<PageCreateUpdateResult> UpdatePageAsync(EditPageViewModel pageViewModel);
        Task<bool> DeletePageByIdAsync(int pageId);
    }
}
