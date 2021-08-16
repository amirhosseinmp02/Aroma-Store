using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.PageModels;
using Microsoft.EntityFrameworkCore;

namespace Aroma_Shop.Data.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly AppDbContext _context;

        public PageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Page> GetPageAsync(int pageId)
        {
            var page =
                await _context
                    .Pages
                    .FindAsync(pageId);

            return page;
        }
        public async Task<Page> GetPageByPathAddressAsync(string pagePathAddress)
        {
            var page =
                await _context
                    .Pages
                    .SingleOrDefaultAsync(p => p.PagePathAddress == pagePathAddress);

            return page;
        }
        public async Task<IEnumerable<Page>> GetPagesAsync()
        {
            var pages =
                await _context
                    .Pages
                    .ToListAsync();

            return pages;
        }
        public async Task<bool> IsPagePathAddressExist(string pagePathAddress)
        {
            var isPagePathAddress =
                await _context
                    .Pages
                    .AnyAsync(p => p.PagePathAddress == pagePathAddress);

            return isPagePathAddress;
        }
        public async Task CreatePageAsync(Page page)
        {
            await _context
                .AddAsync(page);
        }
        public void UpdatePage(Page page)
        {
            _context.Update(page);
        }
        public void DeletePage(Page page)
        {
            _context.Remove(page);
        }
        public async Task SaveAsync()
        {
            await _context
                .SaveChangesAsync();
        }
    }
}
