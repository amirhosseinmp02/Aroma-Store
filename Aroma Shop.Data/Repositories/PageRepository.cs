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
        public void AddPage(Page page)
        {
            _context.Add(page);
        }
        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
