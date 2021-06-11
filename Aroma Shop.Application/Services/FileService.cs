using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.File;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.FileModels;
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

        public JsonResult UploadEditorFile(IFormFile file)
        {
            try
            {
                if (file.Length <= 0)
                    return null;

                var fileName =
                    Guid.NewGuid() + Path.GetExtension(file.FileName).ToLower();

                var filePath =
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","img", "Editor", fileName);

                using (var stream = new FileStream(filePath,FileMode.Create))
                {
                    file.CopyTo(stream);
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
                Console.WriteLine(error);
                return null;
            }
        }
        public IEnumerable<Image> GetProductImagesByIds(IEnumerable<int> productImagesIds)
        {
            var productImages =
                new List<Image>();

            foreach (var productImagesId in productImagesIds)
            {
                var productImage =
                    _fileRepository.GetImage(productImagesId);

                productImages.Add(productImage);
            }

            return productImages;
        }
        public bool AddProductImages(Product product, IEnumerable<IFormFile> productImagesFiles)
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
                    Directory.CreateDirectory(productImagesPath);
                }

                foreach (var productImageFile in productImagesFiles)
                {
                    var productImageFileName =
                        $"{Guid.NewGuid().ToString()} - {productImageFile.FileName.ToLower()}";
                    var fullProductImagesPath
                        = Path.Combine(productImagesPath, productImageFileName);

                    using (var stream = new FileStream(fullProductImagesPath, FileMode.Create))
                    {
                        productImageFile.CopyTo(stream);
                    }

                    var productImage = new Image()
                    {
                        ImagePath = $"Products/{monthProductImagesDirName}/{productImageFileName}",
                        Product = product
                    };

                    _fileRepository.AddImage(productImage);
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }
        public bool DeleteProductImagesByIds(IEnumerable<int> productImagesIds)
        {
            try
            {
                var productImages =
                    GetProductImagesByIds(productImagesIds);

                foreach (var productImage in productImages)
                {
                    var imagePath =
                        Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "img", productImage.ImagePath);

                    File.Delete(imagePath);

                    _fileRepository.DeleteImage(productImage);
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
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

                    _fileRepository.DeleteImage(productImage);
                }

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
