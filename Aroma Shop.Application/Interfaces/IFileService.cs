using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.FileModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IFileService
    {
        IEnumerable<Image> GetProductImagesByIds(IEnumerable<int> productImagesIds);
        bool AddProductImages(Product product, IEnumerable<IFormFile> productImagesFiles);
        bool DeleteProductImagesByIds(IEnumerable<int> productImagesIds);
        bool DeleteProductImages(IEnumerable<Image> productImages);
    }
}
