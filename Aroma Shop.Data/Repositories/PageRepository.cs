using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.PageModels;

namespace Aroma_Shop.Data.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly AppDbContext _context;

        public PageRepository(AppDbContext context)
        {
            _context = context;
        }

        public Page GetPage(int pageId)
        {
            var page =
                _context.Pages.Find(pageId);

            return page;
        }
        public IEnumerable<Page> GetPages()
        {
            var pages =
                _context.Pages;

            return pages;
        }
        public void AddPage(Page page)
        {
            _context.Add(page);
        }
        public void UpdatePage(Page page)
        {
            _context.Update(page);
        }
        public void DeletePage(Page page)
        {
            _context.Remove(page);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
