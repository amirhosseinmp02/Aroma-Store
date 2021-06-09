using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.PageModels;

namespace Aroma_Shop.Application.Services
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public IEnumerable<Page> GetPages()
        {
            var pages =
                _pageRepository.GetPages();

            return pages;
        }
        public bool AddPage(Page page)
        {
            try
            {
                _pageRepository.AddPage(page);

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }
    }
}
