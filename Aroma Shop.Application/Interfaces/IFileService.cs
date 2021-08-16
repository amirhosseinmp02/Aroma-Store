using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        Task<JsonResult> UploadEditorFileAsync(IFormFile file);
        Task<IEnumerable<Image>> GetProductImagesByIdsAsync(IEnumerable<int> productImagesIds);
        Task<bool> AddProductImagesAsync(Product product, IEnumerable<IFormFile> productImagesFiles);
        Task<bool> DeleteProductImagesByIdsAsync(IEnumerable<int> productImagesIds);
        bool DeleteProductImages(IEnumerable<Image> productImages);

        Task<bool> AddBannerImageAsync(Banner banner, IFormFile uploadedBannerImage);
        bool DeleteBannerImage(Image bannerImage);
    }
}
