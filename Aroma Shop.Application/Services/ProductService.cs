﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.FileModels;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;
        private readonly IMediaService _mediaService;
        private readonly IAccountService _accountService;

        public ProductService(IProductRepository productRepository, IFileService fileService, IMediaService mediaService, IAccountService accountService)
        {
            _productRepository = productRepository;
            _fileService = fileService;
            _mediaService = mediaService;
            _accountService = accountService;
        }
        public Product GetProduct(int productId)
        {
            var product =
                _productRepository.GetProduct(productId);

            return product;
        }
        public bool AddProduct(AddEditProductViewModel productViewModel)
        {
            try
            {
                var productCategories = new List<Category>();

                var product = new Product()
                {
                    ProductName = productViewModel.ProductName,
                    ProductDescription = productViewModel.ProductDescription,
                    ProductShortDescription = productViewModel.ProductShortDescription,
                    IsSimpleProduct = productViewModel.IsSimpleProduct,
                    Categories = productCategories,
                    RegistrationTime = DateTime.Now
                };

                if (productViewModel.ProductCategoriesId.Any())
                {
                    AddProductCategories(product, productViewModel.ProductCategoriesId);
                }

                if (productViewModel.ProductImagesFiles.Any())
                    _fileService.AddProductImages(product, productViewModel.ProductImagesFiles);

                if (productViewModel.InformationNames.Any() && productViewModel.InformationValues.Any())
                    AddProductsInformation
                        (product, productViewModel.InformationNames, productViewModel.InformationValues);

                _productRepository.AddProduct(product);
                _productRepository.Save();

                if (productViewModel.IsSimpleProduct)
                {
                    product.ProductPrice = productViewModel.ProductPrice;

                    product.ProductQuantityInStock = productViewModel.ProductQuantityInStock;

                    _productRepository.Save();
                }
                else
                {
                    var result =
                        AddProductAttributes(product, productViewModel);

                    if (!result)
                    {
                        DeleteProduct(product);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
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

                if (productViewModel.ProductCategoriesId.Any())
                    UpdateProductCategories
                        (product, productViewModel.ProductCategoriesId);

                else if (product.Categories.Any())
                    product.Categories.Clear();

                if (productViewModel.ProductImagesFiles.Any())
                    _fileService.AddProductImages(product, productViewModel.ProductImagesFiles);

                if (productViewModel.DeletedProductImagesIds != null)
                    _fileService.DeleteProductImagesByIds(productViewModel.DeletedProductImagesIds);

                if (productViewModel.InformationNames.Any() && productViewModel.InformationValues.Any())
                    UpdateProductsInformation
                        (product, productViewModel.InformationNames, productViewModel.InformationValues);

                else if (product.Informations.Any())
                    DeleteProductInformation(product);

                _productRepository.UpdateProduct(product);
                _productRepository.Save();

                if (productViewModel.IsSimpleProduct)
                {
                    if (!product.IsSimpleProduct)
                    {
                        if (product.ProductAttributesNames.Any())
                            DeleteProductAttributes(product);

                        product.IsSimpleProduct = true;
                    }

                    product.ProductPrice = productViewModel.ProductPrice;

                    product.ProductQuantityInStock = productViewModel.ProductQuantityInStock;
                }
                else
                {
                    product.IsSimpleProduct = false;
                    product.ProductPrice = 0;
                    product.ProductQuantityInStock = 0;

                    if (!product.IsSimpleProduct)
                        DeleteProductAttributes(product);

                    _productRepository.Save();

                    var result =
                        AddProductAttributesForUpdate(product, productViewModel);

                    if (!result)
                        return false;

                }

                _productRepository.UpdateProduct(product);
                _productRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool DeleteProduct(Product product)
        {
            try
            {
                if (product == null)
                    return false;

                if (!product.IsSimpleProduct)
                    DeleteProductAttributes(product);

                if (product.Images.Any())
                    _fileService.DeleteProductImages(product.Images);

                if (product.Informations.Any())
                    DeleteProductInformation(product);

                foreach (var productComment in product.Comments)
                {
                    _mediaService.DeleteComment(productComment);
                }

                _productRepository.DeleteProduct(product);
                _productRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool DeleteProductById(int productId)
        {
            try
            {
                var product =
                    GetProduct(productId);

                var result =
                    DeleteProduct(product);

                return result;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool AddHitsToProduct(Product product)
        {
            try
            {
                ++product.ProductHits;

                _productRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<AddProductToCartResult> AddProductToCart(Product product, int requestedQuantity = 1, int productVariationId = -1)
        {
            try
            {
                ProductVariation requestedVariation = null;

                if (productVariationId == -1)
                {
                    if (product.ProductQuantityInStock < requestedQuantity)
                        return AddProductToCartResult.Failed;
                }
                else
                {
                    requestedVariation =
                        product.ProductVariations
                            .SingleOrDefault(p => p.ProductVariationId == productVariationId);

                    if (requestedVariation == null
                        || requestedVariation.ProductVariationQuantityInStock < requestedQuantity)
                        return AddProductToCartResult.Failed;
                }

                var loggedUser =
                    await _accountService
                        .GetLoggedUserWithOrders();

                var loggedUserOrder =
                    loggedUser.UserOrders
                        .SingleOrDefault(p => !p.IsFinally);

                if (requestedVariation == null)
                {
                    if (loggedUserOrder != null)
                    {
                        var orderDetails =
                            loggedUserOrder.OrdersDetails
                                .SingleOrDefault(p => p.Product.ProductId == product.ProductId);

                        if (orderDetails != null)
                        {
                            var finalOrderDetailsQuantity =
                                orderDetails.OrderDetailsQuantity + requestedQuantity;

                            if (finalOrderDetailsQuantity > product.ProductQuantityInStock)
                                return AddProductToCartResult.OutOfStock;

                            var orderDetailsTotalPrice =
                                product.ProductPrice * finalOrderDetailsQuantity;

                            orderDetails.OrderDetailsTotalPrice =
                                orderDetailsTotalPrice;

                            orderDetails.OrderDetailsQuantity =
                                finalOrderDetailsQuantity;

                            _productRepository.UpdateOrderDetails(orderDetails);
                        }
                        else
                        {
                            var orderDetailsTotalPrice =
                                product.ProductPrice * requestedQuantity;

                            orderDetails = new OrderDetails()
                            {
                                IsOrderDetailsProductSimple = true,
                                OrderDetailsTotalPrice = orderDetailsTotalPrice,
                                OrderDetailsQuantity = requestedQuantity,
                                Order = loggedUserOrder,
                                Product = product
                            };

                            _productRepository.AddOrderDetails(orderDetails);
                        }
                    }
                    else
                    {
                        loggedUserOrder = new Order()
                        {
                            IsFinally = false,
                            CreateTime = DateTime.Now,
                            OwnerUser = loggedUser
                        };

                        _productRepository.AddOrder(loggedUserOrder);

                        var orderDetailsTotalPrice =
                            product.ProductPrice * requestedQuantity;

                        var orderDetails = new OrderDetails()
                        {
                            IsOrderDetailsProductSimple = true,
                            OrderDetailsTotalPrice = orderDetailsTotalPrice,
                            OrderDetailsQuantity = requestedQuantity,
                            Order = loggedUserOrder,
                            Product = product
                        };

                        loggedUserOrder
                            .OrdersDetails.Add(orderDetails);
                    }
                }
                else
                {
                    if (loggedUserOrder != null)
                    {
                        var orderDetails =
                            loggedUserOrder.OrdersDetails
                                .SingleOrDefault(p => p.ProductVariation?.ProductVariationId == productVariationId);

                        if (orderDetails != null)
                        {
                            var finalOrderDetailsQuantity =
                                orderDetails.OrderDetailsQuantity + requestedQuantity;

                            if (finalOrderDetailsQuantity > product.ProductQuantityInStock)
                                return AddProductToCartResult.OutOfStock;

                            var orderDetailsTotalPrice =
                                requestedVariation.ProductVariationPrice * finalOrderDetailsQuantity;

                            orderDetails.OrderDetailsTotalPrice =
                                orderDetailsTotalPrice;

                            orderDetails.OrderDetailsQuantity =
                                finalOrderDetailsQuantity;

                            _productRepository.UpdateOrderDetails(orderDetails);
                        }
                        else
                        {
                            var orderDetailsTotalPrice =
                                requestedVariation.ProductVariationPrice * requestedQuantity;

                            orderDetails = new OrderDetails()
                            {
                                IsOrderDetailsProductSimple = false,
                                OrderDetailsTotalPrice = orderDetailsTotalPrice,
                                OrderDetailsQuantity = requestedQuantity,
                                Order = loggedUserOrder,
                                Product = product,
                                ProductVariation = requestedVariation
                            };

                            _productRepository.AddOrderDetails(orderDetails);
                        }
                    }
                    else
                    {
                        loggedUserOrder = new Order()
                        {
                            IsFinally = false,
                            CreateTime = DateTime.Now,
                            OwnerUser = loggedUser
                        };

                        _productRepository.AddOrder(loggedUserOrder);

                        var orderDetailsTotalPrice =
                            requestedVariation.ProductVariationPrice * requestedQuantity;

                        var orderDetails = new OrderDetails()
                        {
                            IsOrderDetailsProductSimple = false,
                            OrderDetailsTotalPrice = orderDetailsTotalPrice,
                            OrderDetailsQuantity = requestedQuantity,
                            Order = loggedUserOrder,
                            Product = product,
                            ProductVariation = requestedVariation
                        };

                        loggedUserOrder
                            .OrdersDetails.Add(orderDetails);
                    }
                }

                _productRepository.Save();

                return AddProductToCartResult.Successful;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return AddProductToCartResult.Failed;
            }
        }
        public IEnumerable<Product> GetProducts()
        {
            var products =
                _productRepository.GetProducts();

            return products;
        }
        public IEnumerable<Category> GetCategories()
        {
            var categories =
                _productRepository.GetCategories();

            return categories;
        }
        public Category GetCategory(int categoryId)
        {
            var category =
                _productRepository.GetCategory(categoryId);

            return category;
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

                if (categoryViewModel.ParentCategoryId != -1)
                {
                    var parentCategory =
                        GetCategory(categoryViewModel.ParentCategoryId);

                    if (parentCategory != null)
                        category.ParentCategory = parentCategory;
                }

                _productRepository.AddCategory(category);
                _productRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }

        }
        public bool UpdateCategory(AddEditCategoryViewModel categoryViewModel)
        {
            try
            {
                var category = GetCategory(categoryViewModel.CategoryId);

                category.CategoryName = categoryViewModel.CategoryName;
                category.CategoryDescription = categoryViewModel.CategoryDescription;

                var parentCategory =
                    categoryViewModel.ParentCategoryId == -1
                        ? null
                        : GetCategory(categoryViewModel.ParentCategoryId);

                category.ParentCategory = parentCategory;

                _productRepository.UpdateCategory(category);
                _productRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public bool DeleteCategoryById(int categoryId)
        {
            try
            {
                DeleteCascadeCategoryById(categoryId);
                _productRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public IEnumerable<SelectListItem> GetCategoriesTreeView()
        {
            var categories =
                _productRepository.GetCategories();

            List<SelectListItem> items =
                new List<SelectListItem>();

            var item = new SelectListItem("انتخاب کنید", "-1");

            items.Add(item);

            var parentsCategories =
                categories.Where(p => p.ParentCategory == null);

            int count = 0;

            ParentsCategoriesScrolling(parentsCategories);

            void ParentsCategoriesScrolling(IEnumerable<Category> parents)
            {
                foreach (var parent in parents)
                {
                    items
                        .Add
                            (new SelectListItem
                            (new string('─', count * 2)
                             + $" {parent.CategoryName}", parent.CategoryId.ToString()));

                    var category =
                        _productRepository.GetCategory(parent.CategoryId);

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
                    items
                        .Add(new SelectListItem
                            (new string('─', counter * 2)
                             + $" {child.CategoryName}", child.CategoryId.ToString()));

                    var category =
                        _productRepository.GetCategory(child.CategoryId);

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
            var categories =
                _productRepository.GetCategories();

            List<SelectListItem> items =
                new List<SelectListItem>();

            var item = new SelectListItem("انتخاب کنید", "-1");

            items.Add(item);

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
                        var item = new SelectListItem
                        (new string('─', count * 2) +
                         $" {parent.CategoryName}", parent.CategoryId.ToString());

                        items.Add(item);

                        var category =
                            _productRepository.GetCategory(parent.CategoryId);

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

                        var category =
                            _productRepository.GetCategory(child.CategoryId);

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
        public async Task<bool> IsProductInLoggedUserFavoriteProducts(int favoriteProductId)
        {
            try
            {
                var favoriteProduct =
                    GetProduct(favoriteProductId);

                if (favoriteProduct == null)
                    return false;

                var loggedUser =
                    await _accountService.GetLoggedUserWithDetails();

                var isProductInLoggedUserFavoriteProducts =
                    loggedUser.FavoriteProducts
                        .Any(p => p.ProductId == favoriteProductId);

                return isProductInLoggedUserFavoriteProducts;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> AddProductByIdToLoggedUserFavoriteProducts(int favoriteProductId)
        {
            try
            {
                var favoriteProduct =
                    GetProduct(favoriteProductId);

                if (favoriteProduct == null)
                    return false;

                var loggedUser =
                    await _accountService
                        .GetLoggedUserWithDetails();

                var isFavoriteProductExistInUserFavoriteList =
                    loggedUser.FavoriteProducts
                        .Any(p => p.ProductId == favoriteProductId);

                if (isFavoriteProductExistInUserFavoriteList)
                    return false;

                loggedUser
                .FavoriteProducts.Add(favoriteProduct);

                _productRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> RemoveProductByIdFromLoggedUserFavoriteProducts(int favoriteProductId)
        {
            try
            {
                var favoriteProduct =
                    GetProduct(favoriteProductId);

                if (favoriteProduct == null)
                    return false;

                var loggedUser =
                    await _accountService.GetLoggedUserWithDetails();

                var isFavoriteProductExistInUserFavoriteList =
                    loggedUser.FavoriteProducts
                        .Any(p => p.ProductId == favoriteProductId);

                if (!isFavoriteProductExistInUserFavoriteList)
                    return false;

                loggedUser
                    .FavoriteProducts
                    .Remove(favoriteProduct);

                _productRepository.Save();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<IEnumerable<Product>> GetLoggedUserFavoriteProducts()
        {
            var loggedUser =
                await _accountService.GetLoggedUserWithDetails();

            var loggedUserFavoriteProducts =
                loggedUser.FavoriteProducts;

            return loggedUserFavoriteProducts;
        }

        //Utilities Methods

        private bool DeleteCascadeCategoryById(int categoryId)
        {
            try
            {
                var category =
                    GetCategory(categoryId);
                if (category == null)
                    return false;

                _productRepository.DeleteCategory(category);

                if (category.ChildrenCategories.Count != 0)
                {
                    ChildrenCategoriesScrolling(category.ChildrenCategories);

                    void ChildrenCategoriesScrolling(IEnumerable<Category> children)
                    {
                        foreach (var child in children)
                        {
                            _productRepository.DeleteCategory(child);

                            var temp =
                                GetCategory(child.CategoryId);

                            if (temp.ChildrenCategories.Count != 0)
                                ChildrenCategoriesScrolling(temp.ChildrenCategories);
                        }
                    }
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private bool AddProductCategories(Product product, IEnumerable<int> productCategoriesId)
        {
            try
            {
                foreach (var productCategoryId in productCategoriesId)
                {
                    var productCategory =
                        GetCategory(productCategoryId);

                    product.Categories.Add(productCategory);
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private bool UpdateProductCategories(Product product, IEnumerable<int> productCategoriesId)
        {
            try
            {
                product.Categories.Clear();

                AddProductCategories(product, productCategoriesId);

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private bool AddProductsInformation(Product product, IEnumerable<string> informationsNames, IEnumerable<string> informationsValues)
        {
            try
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

                        _productRepository.AddProductInformation(productInformations);
                    }
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private bool DeleteProductInformation(Product product)
        {
            try
            {
                foreach (var productInformation in product.Informations)
                {
                    _productRepository.DeleteProductInformation(productInformation);
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private bool UpdateProductsInformation(Product product, IEnumerable<string> informationsNames, IEnumerable<string> informationsValues)
        {
            try
            {
                if (product.Informations.Any())
                    DeleteProductInformation(product);

                AddProductsInformation(product, informationsNames, informationsValues);

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private bool AddProductAttributes(Product product, AddEditProductViewModel productViewModel)
        {
            try
            {
                product.ProductAttributesNames =
                    productViewModel
                        .AttributesNames.ToList();

                product.ProductAttributesValues =
                    productViewModel
                        .AttributesValues.ToList();

                _productRepository.Save();

                for (int i = 0; i < productViewModel.ProductVariationsNames.Count(); i++)
                {
                    var productVariationValues =
                        productViewModel
                            .ProductVariationsNames
                            .ElementAtOrDefault(i)
                            .Split("-").ToList();

                    var productVariationPrice =
                        productViewModel.ProductVariationsPrices.ElementAtOrDefault(i) >= 0 &&
                        productViewModel.ProductVariationsPrices.ElementAtOrDefault(i) != null
                            ? Convert.ToInt32(productViewModel.ProductVariationsPrices.ElementAtOrDefault(i))
                            : 0;

                    var productVariationQuantityInStock =
                        productViewModel.ProductVariationsQuantityInStocks.ElementAtOrDefault(i) >= 0 &&
                        productViewModel.ProductVariationsQuantityInStocks.ElementAtOrDefault(i) != null
                            ? Convert.ToInt32(productViewModel.ProductVariationsQuantityInStocks.ElementAtOrDefault(i))
                            : 0;

                    var productVariation = new ProductVariation()
                    {
                        ProductVariationValues = productVariationValues,
                        ProductVariationPrice = productVariationPrice,
                        ProductVariationQuantityInStock = productVariationQuantityInStock,
                        Product = product
                    };

                    _productRepository.AddProductVariation(productVariation);
                    _productRepository.Save();
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private bool AddProductAttributesForUpdate(Product product, AddEditProductViewModel productViewModel)
        {
            try
            {
                product.ProductAttributesNames =
                    productViewModel
                        .AttributesNames.ToList();

                product.ProductAttributesValues =
                    productViewModel
                        .AttributesValues.ToList();

                _productRepository.UpdateProduct(product);
                _productRepository.Save();

                for (int i = 0; i < productViewModel.ProductVariationsNames.Count(); i++)
                {
                    var productVariationValues =
                        productViewModel
                            .ProductVariationsNames
                            .ElementAtOrDefault(i)
                            .Split("-").ToList();

                    var productVariationPrice =
                        productViewModel.ProductVariationsPrices.ElementAtOrDefault(i) >= 0 &&
                        productViewModel.ProductVariationsPrices.ElementAtOrDefault(i) != null
                            ? Convert.ToInt32(productViewModel.ProductVariationsPrices.ElementAtOrDefault(i))
                            : 0;

                    var productVariationQuantityInStock =
                        productViewModel.ProductVariationsQuantityInStocks.ElementAtOrDefault(i) >= 0 &&
                        productViewModel.ProductVariationsQuantityInStocks.ElementAtOrDefault(i) != null
                            ? Convert.ToInt32(productViewModel.ProductVariationsQuantityInStocks.ElementAtOrDefault(i))
                            : 0;

                    var productVariation = new ProductVariation()
                    {
                        ProductVariationValues = productVariationValues,
                        ProductVariationPrice = productVariationPrice,
                        ProductVariationQuantityInStock = productVariationQuantityInStock,
                        Product = product
                    };

                    _productRepository.AddProductVariation(productVariation);
                    _productRepository.UpdateProduct(product);
                    _productRepository.Save();
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private bool DeleteProductAttributes(Product product)
        {
            try
            {
                product.ProductAttributesNames = null;
                product.ProductAttributesValues = null;

                foreach (var productVariation in product.ProductVariations)
                {
                    _productRepository.DeleteProductVariation(productVariation);
                }

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
