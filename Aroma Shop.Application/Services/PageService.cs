using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.Page;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace Aroma_Shop.Application.Services
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public Page GetPage(int pageId)
        {
            var page =
                _pageRepository.GetPage(pageId);

            return page;
        }
        public Page GetPageByPathAddress(string pagePathAddress)
        {
            var page =
                GetPages()
                    .SingleOrDefault(p => p.PagePathAddress == pagePathAddress);

            return page;
        }
        public IEnumerable<Page> GetPages()
        {
            var pages =
                _pageRepository.GetPages();

            return pages;
        }
        public JsonResult IsPagePathAddressExistForAddJsonResult(string pagePathAddress)
        {
            var isPagePathAddressExist =
                GetPages()
                    .Any(p => p.PagePathAddress == pagePathAddress);

            if (!isPagePathAddressExist)
                return new JsonResult(true);

            return new JsonResult("این آدرس صفحه در حال حاضر موجود است");
        }
        public JsonResult IsPagePathAddressExistForEditJsonResult(string newPagePathAddress, int pageId)
        {
            var currentPage =
                GetPage(pageId);

            if (currentPage.PagePathAddress != newPagePathAddress)
            {
                var isNewPagePathAddressExist =
                    GetPages()
                        .Any(p => p.PagePathAddress == newPagePathAddress);

                if (isNewPagePathAddressExist)
                    return new JsonResult("این آدرس صفحه در حال حاضر موجود است");

            }

            return new JsonResult(true);
        }
        public PageCreateUpdateResult CreatePage(AddPageViewModel pageViewModel)
        {
            try
            {
                var isPagePathAddressExist =
                    GetPages()
                        .Any(p => p.PagePathAddress == pageViewModel.PagePathAddress);

                if (isPagePathAddressExist)
                    return PageCreateUpdateResult.PathAddressExist;

                var page = new Page()
                {
                    PageTitle = pageViewModel.PageTitle,
                    PagePathAddress = pageViewModel.PagePathAddress,
                    PageDescription = pageViewModel.PageDescription
                };

                _pageRepository.CreatePage(page);

                _pageRepository.Save();

                return PageCreateUpdateResult.Successful;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return PageCreateUpdateResult.Failed;
            }
        }
        public PageCreateUpdateResult UpdatePage(EditPageViewModel pageViewModel)
        {
            try
            {
                var currentPage =
                    GetPage(pageViewModel.PageId);

                if (currentPage.PagePathAddress != pageViewModel.PagePathAddress)
                {
                    var isNewPagePathAddressExist =
                        GetPages()
                            .Any(p => p.PagePathAddress == pageViewModel.PagePathAddress);

                    if (isNewPagePathAddressExist)
                        return PageCreateUpdateResult.PathAddressExist;
                }

                currentPage.PageTitle = pageViewModel.PageTitle;
                currentPage.PagePathAddress = pageViewModel.PagePathAddress;
                currentPage.PageDescription = pageViewModel.PageDescription;

                _pageRepository.UpdatePage(currentPage);

                _pageRepository.Save();

                return PageCreateUpdateResult.Successful;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return PageCreateUpdateResult.Failed;
            }
        }
        public bool DeletePageById(int pageId)
        {
            try
            {
                var page =
                    GetPage(pageId);

                if (page == null)
                    return false;

                _pageRepository.DeletePage(page);
                _pageRepository.Save();

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
