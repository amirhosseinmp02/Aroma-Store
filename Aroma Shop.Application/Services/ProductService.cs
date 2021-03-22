using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Product GetProduct(int productId)
        {
            return _productRepository.GetProduct(productId);
        }

        public bool AddProduct(AddEditProductViewModel productViewModel)
        {
            try
            {
                var productCategories = new List<Category>();
                foreach (var productCategoryId in productViewModel.ProductCategoriesId)
                {
                    var productCategory = GetCategory(productCategoryId);
                    productCategories.Add(productCategory);
                }
                var product = new Product()
                {
                    ProductName = productViewModel.ProductName,
                    ProductDescription = productViewModel.ProductDescription,
                    ProductShortDescription = productViewModel.ProductShortDescription,
                    ProductPrice = productViewModel.ProductPrice,
                    ProductQuantityInStock = productViewModel.ProductQuantityInStock,
                    Categories = productCategories
                };
                if (productViewModel.ProductImagesFiles.Any())
                    AddProductImages(product, productViewModel.ProductImagesFiles);
                if (productViewModel.InformationsNames.Any() && productViewModel.InformationsValues.Any())
                    AddProductsInformations(product, productViewModel.InformationsNames, productViewModel.InformationsValues);
                _productRepository.AddProduct(product);
                _productRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateProduct(AddEditProductViewModel productViewModel)
        {
            try
            {

                var product = GetProduct(productViewModel.ProductId);
                product.ProductName = productViewModel.ProductName;
                product.ProductShortDescription = productViewModel.ProductShortDescription;
                product.ProductDescription = productViewModel.ProductDescription;
                product.ProductPrice = productViewModel.ProductPrice;
                product.ProductQuantityInStock = productViewModel.ProductQuantityInStock;
                if (productViewModel.ProductCategoriesId.Any())
                    UpdateProductCategories(product, productViewModel.ProductCategoriesId);
                else if(product.Categories.Any())
                    product.Categories.Clear();
                if (productViewModel.ProductImagesFiles.Any())
                    AddProductImages(product, productViewModel.ProductImagesFiles);
                if (productViewModel.DeletedProductImagesIds != null)
                    DeleteProductImages(productViewModel.DeletedProductImagesIds);
                if (productViewModel.InformationsNames.Any() && productViewModel.InformationsValues.Any())
                    UpdateProductsInformations(product, productViewModel.InformationsNames, productViewModel.InformationsValues);
                else if (product.ProductName.Any())
                    DeleteProductInformations(product);
                _productRepository.UpdateProduct(product);
                _productRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetProducts();
        }

        public void UpdateProductCategories(Product product, IEnumerable<int> productCategoriesId)
        {
            var productCategories = GetCategories()
                    .Where(p => productCategoriesId
                        .Any(t => p.CategoryId == t)).ToList();
            product.Categories = productCategories;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _productRepository.GetCategories();
        }

        public Category GetCategory(int categoryId)
        {
            return _productRepository.GetCategory(categoryId);
        }

        public bool AddCategory(AddEditCategoryViewModel categoryViewModel)
        {
            try
            {
                var category = new Category()
                {
                    CategoryName = categoryViewModel.CategoryName,
                    CategoryDescription = categoryViewModel.CategoryDescription
                };
                if (categoryViewModel.ParentCategoryId != null)
                {
                    var parentCategory =
                        _productRepository
                            .GetCategory((int)categoryViewModel.ParentCategoryId);
                    category.ParentCategory = parentCategory;
                }
                _productRepository.AddCategory(category);
                _productRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool UpdateCategory(AddEditCategoryViewModel categoryViewModel)
        {
            try
            {
                var parentCategory =
                    GetCategory(Convert.ToInt32(categoryViewModel.ParentCategoryId));
                var category = GetCategory(categoryViewModel.CategoryId);
                category.CategoryName = categoryViewModel.CategoryName;
                category.CategoryDescription = categoryViewModel.CategoryDescription;
                category.ParentCategory = parentCategory;
                _productRepository.UpdateCategory(category);
                _productRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCategory(int categoryId)
        {
            try
            {
                _productRepository.DeleteCategoryById(categoryId);
                _productRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<SelectListItem> GetCategoriesTreeView()
        {
            var categories = _productRepository.GetCategories();
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem("انتخاب کنید", ""));
            var parentsCategories =
                categories.Where(p => p.ParentCategory == null);
            int count = 0;
            ParentsCategoriesScrolling(parentsCategories);
            void ParentsCategoriesScrolling(IEnumerable<Category> parents)
            {
                foreach (var parent in parents)
                {
                    items.Add(new SelectListItem(new string('─', count * 2) + $" {parent.CategoryName}", parent.CategoryId.ToString()));
                    var category = _productRepository.GetCategory(parent.CategoryId);
                    if (category.ChildrenCategories.Any())
                    {
                        ++count;
                        ChildrenCategoriesScrolling(category.ChildrenCategories, count);
                    }
                    count = 0;
                }
            }
            void ChildrenCategoriesScrolling(IEnumerable<Category> children, int counter)
            {
                foreach (var child in children)
                {
                    items.Add(new SelectListItem(new string('─', counter * 2) + $" {child.CategoryName}", child.CategoryId.ToString()));
                    var category = _productRepository.GetCategory(child.CategoryId);
                    if (category.ChildrenCategories.Any())
                    {
                        ++counter;
                        ChildrenCategoriesScrolling(category.ChildrenCategories, counter);
                        --counter;
                    }

                }
            }
            return items;
        }

        public IEnumerable<SelectListItem> GetCategoriesTreeViewForEdit(Category selfCategory)
        {
            var categories = _productRepository.GetCategories();
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem("انتخاب کنید", ""));
            var parentsCategories =
                categories.Where(p => p.ParentCategory == null);
            int count = 0;
            ParentsCategoriesScrolling(parentsCategories);
            void ParentsCategoriesScrolling(IEnumerable<Category> parents)
            {
                foreach (var parent in parents)
                {
                    if (parent.CategoryId != selfCategory.CategoryId)
                    {
                        items.Add
                            (new SelectListItem
                            (new string('─', count * 2) +
                             $" {parent.CategoryName}", parent.CategoryId.ToString()));
                        var category = _productRepository.GetCategory(parent.CategoryId);
                        if (category.ChildrenCategories.Any())
                        {
                            ++count;
                            ChildrenCategoriesScrolling(category.ChildrenCategories, count);
                        }
                        count = 0;
                    }
                }
            }
            void ChildrenCategoriesScrolling(IEnumerable<Category> children, int counter)
            {
                foreach (var child in children)
                {
                    if (child.CategoryId != selfCategory.CategoryId)
                    {
                        items.Add(new SelectListItem
                            (new string('─', counter * 2) +
                             $" {child.CategoryName}", child.CategoryId.ToString()));
                        var category = _productRepository.GetCategory(child.CategoryId);
                        if (category.ChildrenCategories.Any())
                        {
                            ++counter;
                            ChildrenCategoriesScrolling(category.ChildrenCategories, counter);
                            --counter;
                        }
                    }
                }
            }

            if (selfCategory.ParentCategory != null)
            {
                items
                    .SingleOrDefault
                    (p =>
                        p.Value == selfCategory.ParentCategory.CategoryId.ToString())
                    .Selected = true;
            }
            return items;
        }

        public IEnumerable<Image> GetProductImagesByIds(IEnumerable<int> productImagesIds)
        {
            var productImages = new List<Image>();
            foreach (var productImagesId in productImagesIds)
            {
                var productImage = _productRepository.GetImage(productImagesId);
                productImages.Add(productImage);
            }
            return productImages;
        }

        public void AddProductImages(Product product, IEnumerable<IFormFile> productImagesFiles)
        {
            var persianCalendar = new PersianCalendar();
            var monthProductImagesDirName =
                $"{persianCalendar.GetYear(DateTime.Now)} - {persianCalendar.GetMonth(DateTime.Now)}";
            var rootPath =
                Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot", "img", "Product");
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
                    $"{Guid.NewGuid().ToString()} - {productImageFile.FileName}";
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
                _productRepository.AddImage(productImage);
            }
        }

        public void DeleteProductImages(IEnumerable<int> productImagesIds)
        {
            var productImages = GetProductImagesByIds(productImagesIds);
            foreach (var productImage in productImages)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot", "img", "Product", productImage.ImagePath);
                File.Delete(imagePath);
                _productRepository.DeleteImage(productImage);
            }
        }

        public void AddProductsInformations(Product product, IEnumerable<string> informationsNames, IEnumerable<string> informationsValues)
        {
            if (informationsNames.Any() && informationsValues.Any())
            {
                for (int i = 0; i < informationsNames.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(informationsNames.ElementAtOrDefault(i))
                        || !string.IsNullOrEmpty(informationsValues.ElementAtOrDefault(i)))
                    {
                        var productInformations = new ProductInformation()
                        {
                            Name = informationsNames.ElementAtOrDefault(i),
                            Value = informationsValues.ElementAtOrDefault(i),
                            Product = product
                        };
                        _productRepository.AddProductInformations(productInformations);
                    }
                }
            }
        }

        public void DeleteProductInformations(Product product)
        {
            foreach (var productInformation in product.Informations)
            {
                _productRepository.DeleteProductInformation(productInformation);
            }
        }


        public void UpdateProductsInformations(Product product, IEnumerable<string> informationsNames, IEnumerable<string> informationsValues)
        {
            DeleteProductInformations(product);
            AddProductsInformations(product, informationsNames, informationsValues);
        }

    }
}
