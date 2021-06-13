using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.PageModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IPageRepository : IGeneralRepository
    {
        Page GetPage(int pageId);
        IEnumerable<Page> GetPages();
        void AddPage(Page page);
        void UpdatePage(Page page);
        void DeletePage(Page page);
    }
}
