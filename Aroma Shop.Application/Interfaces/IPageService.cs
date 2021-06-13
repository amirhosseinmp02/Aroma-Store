using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.PageModels;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IPageService
    {
        IEnumerable<Page> GetPages();
        bool AddPage(Page page);
        bool DeletePage(Page page);
    }
}
