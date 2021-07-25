using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Application.ViewModels.Banner;
using Aroma_Shop.Domain.Models.FileModels;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IFileService
    {
        JsonResult UploadEditorFile(IFormFile file);
        IEnumerable<Image> GetProductImagesByIds(IEnumerable<int> productImagesIds);
        bool AddProductImages(Product product, IEnumerable<IFormFile> productImagesFiles);
        bool DeleteProductImagesByIds(IEnumerable<int> productImagesIds);
        bool DeleteProductImages(IEnumerable<Image> productImages);

        bool AddBannerImage(Banner banner, AddEditBannerViewModel bannerViewModel);
        bool DeleteBannerImage(Image bannerImage);
    }
}
