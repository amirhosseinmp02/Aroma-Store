using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<Page> GetPageAsync(int pageId)
        {
            var page =
                await _pageRepository
                    .GetPageAsync(pageId);

            return page;
        }
        public async Task<Page> GetPageByPathAddressAsync(string pagePathAddress)
        {
            var page =
                await _pageRepository
                    .GetPageByPathAddressAsync(pagePathAddress);

            return page;
        }
        public async Task<IEnumerable<Page>> GetPagesAsync()
        {
            var pages =
                await _pageRepository
                    .GetPagesAsync();

            return pages;
        }
        public async Task<JsonResult> IsPagePathAddressExistForAddJsonResultAsync(string pagePathAddress)
        {
            var isPagePathAddressExist =
                await _pageRepository
                    .IsPagePathAddressExist(pagePathAddress);

            if (isPagePathAddressExist)
                return new JsonResult("این آدرس صفحه در حال حاضر موجود است");

            return new JsonResult(true);
        }
        public async Task<JsonResult> IsPagePathAddressExistForEditJsonResultAsync(string newPagePathAddress, int pageId)
        {
            var currentPage =
                await GetPageAsync(pageId);

            if (currentPage.PagePathAddress != newPagePathAddress)
            {
                var isNewPagePathAddressExist =
                    await _pageRepository
                        .IsPagePathAddressExist(newPagePathAddress);

                if (isNewPagePathAddressExist)
                    return new JsonResult("این آدرس صفحه در حال حاضر موجود است");
            }

            return new JsonResult(true);
        }
        public async Task<PageCreateUpdateResult> CreatePageAsync(AddPageViewModel pageViewModel)
        {
            try
            {
                var isPagePathAddressExist =
                    await _pageRepository
                        .IsPagePathAddressExist(pageViewModel.PagePathAddress);

                if (isPagePathAddressExist)
                    return PageCreateUpdateResult.PathAddressExist;

                var page = new Page()
                {
                    PageTitle = pageViewModel.PageTitle,
                    PagePathAddress = pageViewModel.PagePathAddress,
                    PageDescription = pageViewModel.PageDescription
                };

                await _pageRepository
                    .CreatePageAsync(page);

                await _pageRepository
                    .SaveAsync();

                return PageCreateUpdateResult.Successful;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return PageCreateUpdateResult.Failed;
            }
        }
        public async Task<PageCreateUpdateResult> UpdatePageAsync(EditPageViewModel pageViewModel)
        {
            try
            {
                var currentPage =
                    await GetPageAsync(pageViewModel.PageId);

                if (currentPage.PagePathAddress != pageViewModel.PagePathAddress)
                {
                    var isNewPagePathAddressExist =
                        await _pageRepository
                            .IsPagePathAddressExist(pageViewModel.PagePathAddress);

                    if (isNewPagePathAddressExist)
                        return PageCreateUpdateResult.PathAddressExist;
                }

                currentPage.PageTitle = pageViewModel.PageTitle;
                currentPage.PagePathAddress = pageViewModel.PagePathAddress;
                currentPage.PageDescription = pageViewModel.PageDescription;

                _pageRepository
                    .UpdatePage(currentPage);

                await _pageRepository
                    .SaveAsync();

                return PageCreateUpdateResult.Successful;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return PageCreateUpdateResult.Failed;
            }
        }
        public async Task<bool> DeletePageByIdAsync(int pageId)
        {
            try
            {
                var page =
                    await GetPageAsync(pageId);

                if (page == null)
                    return false;

                _pageRepository
                    .DeletePage(page);

                await _pageRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
    }
}
