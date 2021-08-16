using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Domain.Models.PageModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IPageRepository : IGeneralRepository
    {
        Task<Page> GetPageAsync(int pageId);
        Task<Page> GetPageByPathAddressAsync(string pagePathAddress);
        Task<IEnumerable<Page>> GetPagesAsync();
        Task<bool> IsPagePathAddressExist(string pagePathAddress);
        Task CreatePageAsync(Page page);
        void UpdatePage(Page page);
        void DeletePage(Page page);
    }
}
