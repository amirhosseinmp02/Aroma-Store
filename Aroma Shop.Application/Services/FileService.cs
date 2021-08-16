using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.Banner;
using Aroma_Shop.Application.ViewModels.File;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.FileModels;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aroma_Shop.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<JsonResult> UploadEditorFileAsync(IFormFile file)
        {
            try
            {
                if (file.Length <= 0)
                    return null;

                var fileName =
                    Guid.NewGuid() + Path.GetExtension(file.FileName).ToLower();

                var filePath =
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Editor", fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file
                        .CopyToAsync(stream);
                }

                var url = $"/img/Editor/{fileName}";

                var successfulResult = new SuccessfulUploadResult()
                {
                    Uploaded = 1,
                    FileName = fileName,
                    Url = url
                };

                var successfulJsonResult =
                    new JsonResult(successfulResult);

                return successfulJsonResult;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return null;
            }
        }
        public async Task<IEnumerable<Image>> GetProductImagesByIdsAsync(IEnumerable<int> productImagesIds)
        {
            var productImages =
                new List<Image>();

            foreach (var productImagesId in productImagesIds)
            {
                var productImage =
                    await _fileRepository
                        .GetImageAsync(productImagesId);

                productImages
                    .Add(productImage);
            }

            return productImages;
        }
        public async Task<bool> AddProductImagesAsync(Product product, IEnumerable<IFormFile> productImagesFiles)
        {
            try
            {
                var persianCalendar =
                    new PersianCalendar();

                var monthProductImagesDirName =
                    $"{persianCalendar.GetYear(DateTime.Now)} - {persianCalendar.GetMonth(DateTime.Now)}";

                var rootPath =
                    Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "img");

                var productImagesPath =
                    Path.Combine(rootPath, "Products", monthProductImagesDirName);

                var isYearMonthProductImagesDirExists =
                    Directory.Exists(productImagesPath);

                if (!isYearMonthProductImagesDirExists)
                {
                    Directory
                        .CreateDirectory(productImagesPath);
                }

                foreach (var productImageFile in productImagesFiles)
                {
                    var productImageFileName =
                        $"{Guid.NewGuid().ToString()} - {productImageFile.FileName.ToLower()}";

                    var fullProductImagesPath
                        = Path.Combine(productImagesPath, productImageFileName);

                    await using (var stream = new FileStream(fullProductImagesPath, FileMode.Create))
                    {
                        await productImageFile
                            .CopyToAsync(stream);
                    }

                    var productImage = new Image()
                    {
                        ImagePath = $"Products/{monthProductImagesDirName}/{productImageFileName}",
                        Product = product
                    };

                    await _fileRepository
                        .AddImageAsync(productImage);
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> DeleteProductImagesByIdsAsync(IEnumerable<int> productImagesIds)
        {
            try
            {
                var productImages =
                    await GetProductImagesByIdsAsync(productImagesIds);

                foreach (var productImage in productImages)
                {
                    var imagePath =
                        Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "img", productImage.ImagePath);

                    File.Delete(imagePath);

                    _fileRepository.DeleteImageAsync(productImage);
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool DeleteProductImages(IEnumerable<Image> productImages)
        {
            try
            {

                foreach (var productImage in productImages)
                {
                    var imagePath =
                        Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "img", productImage.ImagePath);

                    File.Delete(imagePath);

                    _fileRepository.DeleteImageAsync(productImage);
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> AddBannerImageAsync(Banner banner, IFormFile uploadedBannerImage)
        {
            try
            {
                var bannerImagePath =
                    Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "img", "Banners");

                var isBannerImagePathDirExists =
                    Directory.Exists(bannerImagePath);

                if (!isBannerImagePathDirExists)
                {
                    Directory
                        .CreateDirectory(bannerImagePath);
                }

                var bannerImageFileName =
                    $"{Guid.NewGuid().ToString()} - {uploadedBannerImage.FileName.ToLower()}";

                var fullBannerImagePath
                    = Path.Combine(bannerImagePath, bannerImageFileName);

                await using (var stream = new FileStream(fullBannerImagePath, FileMode.Create))
                {
                    await uploadedBannerImage
                        .CopyToAsync(stream);
                }

                var bannerImage = new Image()
                {
                    ImagePath = $"Banners/{bannerImageFileName}"
                };

                await _fileRepository
                    .AddImageAsync(bannerImage);

                banner
                    .BannerImage = bannerImage;

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return true;
            }
        }

        public bool DeleteBannerImage(Image bannerImage)
        {
            try
            {
                var imagePath =
                    Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "img", bannerImage.ImagePath);

                File.Delete(imagePath);

                _fileRepository
                    .DeleteImageAsync(bannerImage);

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
