using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.PageModels;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IPageService
    {
        Page GetPage(int pageId);
        IEnumerable<Page> GetPages();
        bool AddPage(Page page);
        bool UpdatePage(Page page);
        bool DeletePageById(int pageId);
    }
}
