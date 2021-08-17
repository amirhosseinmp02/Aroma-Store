using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.IdPayModels;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.FileModels;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Aroma_Shop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;
        private readonly IMediaService _mediaService;
        private readonly IAccountService _accountService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _accessor;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProductService(IProductRepository productRepository, IFileService fileService, IMediaService mediaService, IAccountService accountService, LinkGenerator linkGenerator, IHttpContextAccessor accessor, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _fileService = fileService;
            _mediaService = mediaService;
            _accountService = accountService;
            _linkGenerator = linkGenerator;
            _accessor = accessor;
            _configuration = configuration;
            _httpClient = new HttpClient();
        }
        public async Task<IEnumerable<Product>> GetAvailableProductsAsync()
        {
            var availableProducts =
                await _productRepository
                    .GetAvailableProductsAsync();

            return availableProducts;
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            var product =
                await _productRepository
                    .GetProductAsync(productId);

            return product;
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var products =
                await _productRepository
                    .GetProductsAsync();

            return products;
        }
        public async Task<Product> GetProductWithDetailsAsync(int productId)
        {
            var product =
                await _productRepository
                    .GetProductWithDetailsAsync(productId);

            return product;
        }
        public async Task<int> GetProductsCountAsync()
        {
            var productsCount =
                await _productRepository
                    .GetProductsCountAsync();

            return productsCount;
        }
        public async Task<bool> AddProductAsync(AddEditProductViewModel productViewModel)
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
                    await AddProductCategoriesAsync(product, productViewModel.ProductCategoriesId);
                }

                if (productViewModel.ProductImagesFiles.Any())
                    await _fileService
                        .AddProductImagesAsync(product, productViewModel.ProductImagesFiles);

                if (productViewModel.InformationNames.Any() && productViewModel.InformationValues.Any())
                    await AddProductsInformationAsync
                        (product, productViewModel.InformationNames, productViewModel.InformationValues);

                await _productRepository
                    .AddProductAsync(product);

                await _productRepository
                    .SaveAsync();

                if (productViewModel.IsSimpleProduct)
                {
                    product.ProductPrice = productViewModel.ProductPrice;

                    product.ProductQuantityInStock = productViewModel.ProductQuantityInStock;

                    await _productRepository
                        .SaveAsync();
                }
                else
                {
                    var result =
                        await AddProductAttributesAsync(product, productViewModel);

                    if (!result)
                    {
                        await 
                            DeleteProductAsync(product);

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
        public async Task<bool> UpdateProductAsync(AddEditProductViewModel productViewModel)
        {
            try
            {
                var product =
                    await GetProductWithDetailsAsync(productViewModel.ProductId);

                product.ProductName = productViewModel.ProductName;
                product.ProductShortDescription = productViewModel.ProductShortDescription;
                product.ProductDescription = productViewModel.ProductDescription;

                if (productViewModel.ProductCategoriesId.Any())
                    await UpdateProductCategoriesAsync
                        (product, productViewModel.ProductCategoriesId);

                else if (product.Categories.Any())
                    product.Categories.Clear();

                if (productViewModel.ProductImagesFiles.Any())
                    await _fileService
                        .AddProductImagesAsync(product, productViewModel.ProductImagesFiles);

                if (productViewModel.DeletedProductImagesIds != null)
                    await _fileService
                        .DeleteProductImagesByIdsAsync(productViewModel.DeletedProductImagesIds);

                if (productViewModel.InformationNames.Any() && productViewModel.InformationValues.Any())
                    await UpdateProductsInformationAsync
                        (product, productViewModel.InformationNames, productViewModel.InformationValues);

                else if (product.Informations.Any())
                    DeleteProductInformation(product);

                _productRepository
                    .UpdateProduct(product);

                await _productRepository
                    .SaveAsync();

                if (productViewModel.IsSimpleProduct)
                {
                    if (!product.IsSimpleProduct)
                    {
                        if (product.ProductAttributesNames.Any())
                            await DeleteProductAttributesAsync(product);

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
                        await DeleteProductAttributesAsync(product);

                    await _productRepository
                        .SaveAsync();

                    var result =
                        await AddProductAttributesForUpdateAsync(product, productViewModel);

                    if (!result)
                        return false;

                }

                _productRepository
                    .UpdateProduct(product);

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> DeleteProductAsync(Product product)
        {
            try
            {
                if (product == null)
                    return false;

                if (!product.IsSimpleProduct)
                    await DeleteProductAttributesAsync(product);

                if (product.Images.Any())
                    _fileService.DeleteProductImages(product.Images);

                if (product.Informations.Any())
                    DeleteProductInformation(product);

                var productParentsComments =
                    product
                        .Comments
                        .Where(p => p.ParentComment == null);

                _mediaService
                    .DeleteCommentsByParents(productParentsComments);

                _productRepository
                    .DeleteProduct(product);

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> DeleteProductByIdAsync(int productId)
        {
            try
            {
                var product =
                    await GetProductWithDetailsAsync(productId);

                var result =
                    await DeleteProductAsync(product);

                return result;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> AddHitsToProductAsync(Product product)
        {
            try
            {
                ++product.ProductHits;

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categories =
                await _productRepository
                    .GetCategoriesAsync();

            return categories;
        }
        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            var category =
                await _productRepository
                    .GetCategoryAsync(categoryId);

            return category;
        }
        public async Task<bool> AddCategoryAsync(AddEditCategoryViewModel categoryViewModel)
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
                        await GetCategoryAsync(categoryViewModel.ParentCategoryId);

                    if (parentCategory != null)
                        category.ParentCategory = parentCategory;
                }

                await _productRepository
                    .AddCategoryAsync(category);

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }

        }
        public async Task<bool> UpdateCategoryAsync(AddEditCategoryViewModel categoryViewModel)
        {
            try
            {
                var category =
                    await GetCategoryAsync(categoryViewModel.CategoryId);

                category.CategoryName = categoryViewModel.CategoryName;
                category.CategoryDescription = categoryViewModel.CategoryDescription;

                var parentCategory =
                    categoryViewModel.ParentCategoryId == -1
                        ? null
                        : await GetCategoryAsync(categoryViewModel.ParentCategoryId);

                category.ParentCategory = parentCategory;

                _productRepository
                    .UpdateCategory(category);

                await _productRepository.
                    SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> DeleteCategoryByIdAsync(int categoryId)
        {
            try
            {
                await DeleteCascadeCategoryByIdAsync(categoryId);

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<IEnumerable<CategoryTreeView>> GetCategoriesTreeViewsAsync()
        {
            var categories =
                await _productRepository
                    .GetCategoriesAsync();

            List<CategoryTreeView> items =
                new List<CategoryTreeView>();

            var parentsCategories =
                categories
                    .Where(p => p.ParentCategory == null);

            foreach (var parentCategory in parentsCategories)
            {
                var item =
                    new CategoryTreeView()
                    {
                        CategoryId = parentCategory.CategoryId,
                        CategoryName = parentCategory.CategoryName,
                        CategoryLevel = 0,
                        CategoryProductsCount = parentCategory.Products.Count
                    };

                items
                .Add(item);

                CategoriesScrolling(parentCategory);
            }

            void CategoriesScrolling(Category parentCategory, int counter = 0)
            {
                var subCategories =
                    categories
                        .Where(p => p.ParentCategory?.CategoryId == parentCategory.CategoryId);

                foreach (var subCategory in subCategories)
                {
                    ++counter;

                    var item =
                        new CategoryTreeView()
                        {
                            CategoryId = subCategory.CategoryId,
                            CategoryName = subCategory.CategoryName,
                            CategoryLevel = counter,
                            CategoryProductsCount = subCategory.Products.Count
                        };

                    items
                        .Add(item);

                    CategoriesScrolling(subCategory, counter);
                    --counter;
                }
            }

            return items;
        }
        public async Task<IEnumerable<SelectListItem>> GetCategoriesTreeViewForAddAsync()
        {
            var categories =
                await _productRepository
                    .GetCategoriesAsync();

            List<SelectListItem> items =
                new List<SelectListItem>()
                {
                    new SelectListItem("انتخاب کنید", "-1")
                };

            var parentsCategories =
                categories
                    .Where(p => p.ParentCategory == null);

            foreach (var parentCategory in parentsCategories)
            {
                var item =
                    new SelectListItem
                    (new string('─', 0)
                     + $" {parentCategory.CategoryName}", parentCategory.CategoryId.ToString());

                items
                    .Add(item);

                CategoriesScrolling(parentCategory);
            }

            void CategoriesScrolling(Category parentCategory, int counter = 0)
            {
                var subCategories =
                    categories
                        .Where(p => p.ParentCategory?.CategoryId == parentCategory.CategoryId);

                foreach (var subCategory in subCategories)
                {
                    ++counter;

                    var item =
                        new SelectListItem
                        (new string('─', counter * 2)
                         + $" {subCategory.CategoryName}", subCategory.CategoryId.ToString());

                    items
                        .Add(item);

                    CategoriesScrolling(subCategory, counter);
                    --counter;
                }
            }

            return items;
        }
        public async Task<IEnumerable<SelectListItem>> GetCategoriesTreeViewForEditAsync(Category selfCategory)
        {
            var categoriesTreeViewForAdd =
                await GetCategoriesTreeViewForAddAsync();

            var items =
                categoriesTreeViewForAdd.ToList();

            var selfItem =
            items
                .SingleOrDefault
                (p => p.Value ==
                      selfCategory.CategoryId.ToString());

            items.Remove(selfItem);

            return items;
        }
        public async Task<bool> AddProductByIdToLoggedUserFavoriteProductsAsync(int favoriteProductId)
        {
            try
            {
                var favoriteProduct =
                    await GetProductAsync(favoriteProductId);

                if (favoriteProduct == null)
                    return false;

                var loggedUser =
                    await _accountService
                        .GetLoggedUserWithFavoriteProductsAsync();

                var isFavoriteProductExistInUserFavoriteList =
                    loggedUser
                        .FavoriteProducts
                        .Any(p => p.ProductId == favoriteProductId);

                if (isFavoriteProductExistInUserFavoriteList)
                    return false;

                loggedUser
                    .FavoriteProducts.Add(favoriteProduct);

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> RemoveProductByIdFromLoggedUserFavoriteProductsAsync(int favoriteProductId)
        {
            try
            {
                var favoriteProduct =
                    await GetProductAsync(favoriteProductId);

                if (favoriteProduct == null)
                    return false;

                var loggedUser =
                    await _accountService
                        .GetLoggedUserWithDetailsAsync();

                var isFavoriteProductExistInUserFavoriteList =
                    loggedUser
                        .FavoriteProducts
                        .Any(p => p.ProductId == favoriteProductId);

                if (!isFavoriteProductExistInUserFavoriteList)
                    return false;

                loggedUser
                    .FavoriteProducts
                    .Remove(favoriteProduct);

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<IEnumerable<Product>> GetLoggedUserFavoriteProductsAsync()
        {
            var loggedUser =
                await _accountService
                    .GetLoggedUserWithDetailsAsync();

            var loggedUserFavoriteProducts =
                loggedUser.FavoriteProducts;

            return loggedUserFavoriteProducts;
        }

        //Start Order Section

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            var orders =
                await _productRepository
                    .GetOrdersAsync();

            return orders;
        }
        public async Task<IEnumerable<OrdersViewModel>> GetOrdersListViewAsync()
        {
            var orders =
                await _productRepository
                    .GetOrdersAsync();

            var ordersViewModel =
                orders
                    .Select(p =>
                    {
                        var orderName =
                            $"#{p.OrderId} {p.OwnerUser.UserDetails.FirstName} {p.OwnerUser.UserDetails.LastName}";

                        var orderDate =
                            p.OrderPaymentTime == default
                                ? p.OrderCreateTime.ToSolarWithTime()
                                : p.OrderPaymentTime.ToSolarWithTime();

                        int totalOrderPrice;

                        if (p.OrdersDetails.NotNullOrEmpty())
                        {
                            totalOrderPrice =
                                p.OrdersDetails.Sum(p => p.OrderDetailsTotalPrice) -
                                p.Discounts.Sum(p => p.DiscountPrice);
                        }
                        else
                        {
                            totalOrderPrice =
                                p.InvoicesDetails.Sum(p => p.InvoiceDetailsTotalPrice) -
                                p.Discounts.Sum(p => p.DiscountPrice);
                        }

                        var notEmpty =
                            p.OrdersDetails.NotNullOrEmpty() ||
                            p.InvoicesDetails.NotNullOrEmpty();

                        var orderViewModel = new OrdersViewModel()
                        {
                            OrderId = p.OrderId,
                            OrderName = orderName,
                            OrderDate = orderDate,
                            OrderStatus = p.OrderStatus,
                            IsOrderSeen = p.IsOrderSeen,
                            NotEmpty = notEmpty,
                            OrderTotalPrice = totalOrderPrice > 0 ? $"{totalOrderPrice} تومان" : "رایگان"
                        };

                        return orderViewModel;
                    });

            return ordersViewModel;
        }
        public async Task<IEnumerable<OrdersViewModel>> GetLoggedUserOrdersAsync()
        {
            var loggedUserId =
                _accessor.HttpContext
                    .User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedUserOrders =
                await _productRepository
                    .GetUserOrdersAsync(loggedUserId);

            var ordersViewModel =
                loggedUserOrders
                    .Select(p =>
                    {
                        var orderName =
                            $"#{p.OrderId} {p.OwnerUser.UserDetails.FirstName} {p.OwnerUser.UserDetails.LastName}";

                        var orderDate =
                            p.OrderPaymentTime == default
                                ? p.OrderCreateTime.ToSolarWithTime()
                                : p.OrderPaymentTime.ToSolarWithTime();

                        var notEmpty =
                            p.OrdersDetails.NotNullOrEmpty() ||
                            p.InvoicesDetails.NotNullOrEmpty();

                        int totalOrderPrice;

                        if (p.OrdersDetails.NotNullOrEmpty())
                        {
                            totalOrderPrice =
                                p.OrdersDetails.Sum(p => p.OrderDetailsTotalPrice) -
                                p.Discounts.Sum(p => p.DiscountPrice);
                        }
                        else
                        {
                            totalOrderPrice =
                                p.InvoicesDetails.Sum(p => p.InvoiceDetailsTotalPrice) -
                                p.Discounts.Sum(p => p.DiscountPrice);
                        }

                        var orderViewModel = new OrdersViewModel()
                        {
                            OrderId = p.OrderId,
                            OrderName = orderName,
                            OrderDate = orderDate,
                            OrderStatus = p.OrderStatus,
                            OrderTotalPrice = totalOrderPrice > 0 ? $"{totalOrderPrice} تومان" : "رایگان",
                            NotEmpty = notEmpty
                        };

                        return orderViewModel;
                    });

            return ordersViewModel;
        }
        public async Task<OrderViewModel> GetOrderForEditAsync(int orderId)
        {
            var order =
                await _productRepository
                    .GetOrderWithDetailsAsync(orderId);

            if (order == null)
                return null;

            var orderName =
                $"#{order.OrderId} {order.OwnerUser.UserDetails.FirstName} {order.OwnerUser.UserDetails.LastName}";

            var createTime =
                order
                    .OrderCreateTime
                    .ToSolarWithTime();

            var paymentTime =
                order.OrderPaymentTime != default
                    ? order.OrderPaymentTime.ToSolarWithTime() :
                    "-";

            var paymentMethod =
                order.IsOrderCompleted ?
                    "آیدی پی" :
                    "-";

            var orderViewModel = new OrderViewModel()
            {
                OrderName = orderName,
                OrderId = order.OrderId,
                CreateTime = createTime,
                PaymentTime = paymentTime,
                PaymentMethod = paymentMethod,
                OrderStatus = order.OrderStatus,
                OrderNote = order.OrderNote,
                OwnerUser = order.OwnerUser,
                Discounts = order.Discounts
            };

            int totalOrderPrice;

            if (order.OrdersDetails.NotNullOrEmpty())
            {
                totalOrderPrice =
                    order.OrdersDetails.Sum(p => p.OrderDetailsTotalPrice) -
                    order.Discounts.Sum(p => p.DiscountPrice);

                orderViewModel
                        .OrderTotalPrice =
                    totalOrderPrice > 0 ? $"{totalOrderPrice} تومان" : "رایگان";

                orderViewModel
                        .OrderInvoicesDetails =
                    order
                        .OrdersDetails
                        .Select(p =>
                        {
                            var orderInvoiceDetails = new OrderInvoiceDetails()
                            {
                                IsInvoiceDetailsProductSimple = p.IsOrderDetailsProductSimple,
                                InvoiceDetailsProductName = p.Product.ProductName,
                                InvoiceDetailsTotalPrice = p.OrderDetailsTotalPrice,
                                InvoiceDetailsQuantity = p.OrderDetailsQuantity,
                            };

                            if (!p.IsOrderDetailsProductSimple)
                            {
                                orderInvoiceDetails
                                        .InvoiceDetailsProductAttributesNames =
                                    p.Product
                                        .ProductAttributesNames;

                                orderInvoiceDetails
                                        .InvoiceDetailsProductVariationValues =
                                    p.ProductVariation
                                        .ProductVariationValues;
                            }

                            return orderInvoiceDetails;
                        }).ToList();
            }
            else
            {
                totalOrderPrice =
                    order.InvoicesDetails.Sum(p => p.InvoiceDetailsTotalPrice) -
                    order.Discounts.Sum(p => p.DiscountPrice);

                orderViewModel
                        .OrderTotalPrice =
                    totalOrderPrice > 0 ? $"{totalOrderPrice} تومان" : "رایگان";

                orderViewModel
                        .OrderInvoicesDetails =
                    order
                        .InvoicesDetails;
            }

            await SetOrderAsSeenAsync(order);

            return orderViewModel;
        }
        public async Task<Order> GetLoggedUserOpenOrderAsync()
        {
            var loggedUserId =
                _accessor
                    .HttpContext
                    .User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedUserOpenOrder =
                await _productRepository
                    .GetUserOpenOrderAsync(loggedUserId);

            return loggedUserOpenOrder;
        }
        public async Task<OrderViewModel> OrderTrackingByUserEmailAsync(string userEmail, int orderId)
        {
            var userOrder =
                await _productRepository
                    .GetUserOrderByEmailAsync(userEmail, orderId);

            if (userOrder == null)
                return null;

            var orderName =
               $"#{userOrder.OrderId} {userOrder.OwnerUser.UserDetails.FirstName} {userOrder.OwnerUser.UserDetails.LastName}";

            var createTime =
                userOrder
                    .OrderCreateTime
                    .ToSolarWithTime();

            var paymentTime =
                userOrder.OrderPaymentTime != default
                    ? userOrder.OrderPaymentTime.ToSolarWithTime() :
                    "-";

            var paymentMethod =
                userOrder.IsOrderCompleted ?
                    "آیدی پی" :
                    "-";

            var orderViewModel = new OrderViewModel()
            {
                OrderName = orderName,
                OrderId = userOrder.OrderId,
                CreateTime = createTime,
                PaymentTime = paymentTime,
                PaymentMethod = paymentMethod,
                OrderStatus = userOrder.OrderStatus,
                OrderNote = userOrder.OrderNote,
                OwnerUser = userOrder.OwnerUser,
                Discounts = userOrder.Discounts
            };

            int totalOrderPrice;

            if (userOrder.OrdersDetails != null)
            {
                totalOrderPrice =
                    userOrder.OrdersDetails.Sum(p => p.OrderDetailsTotalPrice) -
                    userOrder.Discounts.Sum(p => p.DiscountPrice);

                orderViewModel
                        .OrderTotalPrice =
                    totalOrderPrice > 0 ? $"{totalOrderPrice} تومان" : "رایگان";

                orderViewModel
                        .OrderInvoicesDetails =
                    userOrder
                        .OrdersDetails
                        .Select(p =>
                        {
                            var orderInvoiceDetails = new OrderInvoiceDetails()
                            {
                                IsInvoiceDetailsProductSimple = p.IsOrderDetailsProductSimple,
                                InvoiceDetailsProductName = p.Product.ProductName,
                                InvoiceDetailsTotalPrice = p.OrderDetailsTotalPrice,
                                InvoiceDetailsQuantity = p.OrderDetailsQuantity,
                            };

                            if (!p.IsOrderDetailsProductSimple)
                            {
                                orderInvoiceDetails
                                        .InvoiceDetailsProductAttributesNames =
                                    p.Product
                                        .ProductAttributesNames;

                                orderInvoiceDetails
                                        .InvoiceDetailsProductVariationValues =
                                    p.ProductVariation
                                        .ProductVariationValues;
                            }

                            return orderInvoiceDetails;
                        }).ToList();
            }
            else
            {
                totalOrderPrice =
                    userOrder.InvoicesDetails.Sum(p => p.InvoiceDetailsTotalPrice) -
                    userOrder.Discounts.Sum(p => p.DiscountPrice);

                orderViewModel
                        .OrderTotalPrice =
                    totalOrderPrice > 0 ? $"{totalOrderPrice} تومان" : "رایگان";

                orderViewModel
                        .OrderInvoicesDetails =
                    userOrder
                        .InvoicesDetails;
            }

            return orderViewModel;
        }
        public OrderViewModel GetConfirmedOrderInvoiceAsync(Order confirmedOrder)
        {
            var orderName =
                $"#{confirmedOrder.OrderId} {confirmedOrder.OwnerUser.UserDetails.FirstName} {confirmedOrder.OwnerUser.UserDetails.LastName}";

            var createTime =
                confirmedOrder
                    .OrderCreateTime
                    .ToSolarWithTime();

            var paymentTime =
                confirmedOrder.OrderPaymentTime != default
                    ? confirmedOrder.OrderPaymentTime.ToSolarWithTime() :
                    "-";

            var paymentMethod =
                confirmedOrder.IsOrderCompleted ?
                    "آیدی پی" :
                    "-";

            int totalOrderPrice;

            totalOrderPrice =
                confirmedOrder.InvoicesDetails.Sum(p => p.InvoiceDetailsTotalPrice) -
                confirmedOrder.Discounts.Sum(p => p.DiscountPrice);

            var orderViewModel = new OrderViewModel()
            {
                OrderName = orderName,
                OrderId = confirmedOrder.OrderId,
                CreateTime = createTime,
                PaymentTime = paymentTime,
                PaymentMethod = paymentMethod,
                OrderStatus = confirmedOrder.OrderStatus,
                OrderNote = confirmedOrder.OrderNote,
                OrderTotalPrice = totalOrderPrice > 0 ? $"{totalOrderPrice} تومان" : "رایگان",
                OwnerUser = confirmedOrder.OwnerUser,
                Discounts = confirmedOrder.Discounts,
                OrderInvoicesDetails = confirmedOrder.InvoicesDetails
            };

            return orderViewModel;
        }
        public async Task<OrderViewModel> GetLoggedUserOrderInvoiceAsync(int orderId)
        {
            var loggedUserId =
                _accessor
                    .HttpContext
                    .User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

            var userOrder =
                await _productRepository
                    .GetUserOrderAsync(loggedUserId, orderId);

            if (userOrder == null || (!userOrder.OrdersDetails.NotNullOrEmpty() && !userOrder.InvoicesDetails.NotNullOrEmpty()))
                return null;

            var orderName =
               $"#{userOrder.OrderId} {userOrder.OwnerUser.UserDetails.FirstName} {userOrder.OwnerUser.UserDetails.LastName}";

            var createTime =
                userOrder
                    .OrderCreateTime
                    .ToSolarWithTime();

            var paymentTime =
                userOrder.OrderPaymentTime != default
                    ? userOrder.OrderPaymentTime.ToSolarWithTime() :
                    "-";

            var paymentMethod =
                userOrder.IsOrderCompleted ?
                    "آیدی پی" :
                    "-";

            var orderViewModel = new OrderViewModel()
            {
                OrderName = orderName,
                OrderId = userOrder.OrderId,
                CreateTime = createTime,
                PaymentTime = paymentTime,
                PaymentMethod = paymentMethod,
                OrderStatus = userOrder.OrderStatus,
                OrderNote = userOrder.OrderNote,
                OwnerUser = userOrder.OwnerUser,
                Discounts = userOrder.Discounts
            };

            int totalOrderPrice;

            if (userOrder.OrdersDetails.NotNullOrEmpty())
            {
                totalOrderPrice =
                    userOrder.OrdersDetails.Sum(p => p.OrderDetailsTotalPrice) -
                    userOrder.Discounts.Sum(p => p.DiscountPrice);

                orderViewModel
                        .OrderTotalPrice =
                    totalOrderPrice > 0 ? $"{totalOrderPrice} تومان" : "رایگان";

                orderViewModel
                        .OrderInvoicesDetails =
                    userOrder
                        .OrdersDetails
                        .Select(p =>
                        {
                            var orderInvoiceDetails = new OrderInvoiceDetails()
                            {
                                IsInvoiceDetailsProductSimple = p.IsOrderDetailsProductSimple,
                                InvoiceDetailsProductName = p.Product.ProductName,
                                InvoiceDetailsTotalPrice = p.OrderDetailsTotalPrice,
                                InvoiceDetailsQuantity = p.OrderDetailsQuantity,
                            };

                            if (!p.IsOrderDetailsProductSimple)
                            {
                                orderInvoiceDetails
                                        .InvoiceDetailsProductAttributesNames =
                                    p.Product
                                        .ProductAttributesNames;

                                orderInvoiceDetails
                                        .InvoiceDetailsProductVariationValues =
                                    p.ProductVariation
                                        .ProductVariationValues;
                            }

                            return orderInvoiceDetails;
                        }).ToList();
            }
            else
            {
                totalOrderPrice =
                    userOrder.InvoicesDetails.Sum(p => p.InvoiceDetailsTotalPrice) -
                    userOrder.Discounts.Sum(p => p.DiscountPrice);

                orderViewModel
                        .OrderTotalPrice =
                    totalOrderPrice > 0 ? $"{totalOrderPrice} تومان" : "رایگان";

                orderViewModel
                        .OrderInvoicesDetails =
                    userOrder
                        .InvoicesDetails;
            }

            return orderViewModel;
        }
        public async Task<int> GetCompletedOrdersCountAsync()
        {
            var completedOrdersCount =
                await _productRepository
                    .GetCompletedOrdersCountAsync();

            return completedOrdersCount;
        }
        public async Task<int> GetUnCompletedOrdersCountAsync()
        {
            var unCompletedOrdersCount =
                await _productRepository
                    .GetUnCompletedOrdersCountAsync();

            return unCompletedOrdersCount;
        }
        public async Task<int> GetUnSeenOrdersCountAsync()
        {
            var unSeenOrdersCount =
                await _productRepository
                    .GetUnSeenOrdersCountAsync();

            return unSeenOrdersCount;
        }
        public async Task<bool> UpdateOrderAsync(Order order)
        {
            try
            {
                var currentOrder =
                    await _productRepository
                        .GetOrderAsync(order.OrderId);

                if (currentOrder == null)
                    return false;

                currentOrder
                        .OrderStatus =
                    order
                        .OrderStatus;

                _productRepository
                    .UpdateOrder(currentOrder);

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> SetOrderAsSeenAsync(Order order)
        {
            try
            {
                if (order.IsOrderSeen)
                    return true;

                order.IsOrderSeen = true;

                _productRepository
                    .UpdateOrder(order);

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> DeleteOrderByIdAsync(int orderId)
        {
            try
            {
                var order =
                    await _productRepository
                        .GetOrderAsync(orderId);

                if (order == null)
                    return false;

                _productRepository
                    .DeleteOrder(order);

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<int> GetLoggedUserOpenOrderDetailsCountAsync()
        {
            var loggedUserId =
                _accessor
                    .HttpContext
                    .User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedUserOpenOrderDetailsCount =
                await _productRepository
                    .GetUserOpenOrderDetailsCountAsync(loggedUserId);

            return loggedUserOpenOrderDetailsCount;
        }
        public async Task<bool> DeleteOrderDetailsByIdAsync(int orderDetailsId)
        {
            try
            {
                var orderDetails =
                    await _productRepository
                        .GetOrderDetailsAsync(orderDetailsId);

                if (orderDetails == null)
                    return false;

                _productRepository
                    .DeleteOrderDetails(orderDetails);

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<AddProductToCartResult> AddProductToCartAsync(Product product, int requestedQuantity = 1, int productVariationId = -1)
        {
            try
            {
                ProductVariation requestedVariation = null;

                if (productVariationId == -1)
                {
                    if (product.ProductQuantityInStock < requestedQuantity || 1 > requestedQuantity)
                        return AddProductToCartResult.Failed;
                }
                else
                {
                    requestedVariation =
                        product.ProductVariations
                            .SingleOrDefault(p => p.ProductVariationId == productVariationId);

                    if (requestedVariation == null
                        || requestedVariation.ProductVariationQuantityInStock < requestedQuantity
                        || 1 > requestedQuantity)
                        return AddProductToCartResult.Failed;
                }

                var loggedUserOrder =
                    await GetLoggedUserOpenOrderAsync();

                var loggedUser =
                    await _accountService
                        .GetLoggedUserAsync();

                if (requestedVariation == null)
                {
                    if (loggedUserOrder != null)
                    {
                        var orderDetails =
                            loggedUserOrder
                                .OrdersDetails
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

                            await _productRepository
                                .AddOrderDetailsAsync(orderDetails);
                        }
                    }
                    else
                    {
                        loggedUserOrder = new Order()
                        {
                            IsOrderCompleted = false,
                            OrderCreateTime = DateTime.Now,
                            OrderStatus = OrderStatus.AwaitingPayment.GetDescription(),
                            OwnerUser = loggedUser
                        };

                        await _productRepository
                            .AddOrderAsync(loggedUserOrder);

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
                            loggedUserOrder
                                .OrdersDetails
                                .SingleOrDefault(p => p.ProductVariation?.ProductVariationId == productVariationId);

                        if (orderDetails != null)
                        {
                            var finalOrderDetailsQuantity =
                                orderDetails.OrderDetailsQuantity + requestedQuantity;

                            if (finalOrderDetailsQuantity > requestedVariation.ProductVariationQuantityInStock)
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

                            await _productRepository
                                .AddOrderDetailsAsync(orderDetails);
                        }
                    }
                    else
                    {
                        loggedUserOrder = new Order()
                        {
                            IsOrderCompleted = false,
                            OrderCreateTime = DateTime.Now,
                            OrderStatus = OrderStatus.AwaitingPayment.GetDescription(),
                            OwnerUser = loggedUser
                        };

                        await _productRepository
                            .AddOrderAsync(loggedUserOrder);

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

                await _productRepository
                    .SaveAsync();

                return AddProductToCartResult.Successful;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return AddProductToCartResult.Failed;
            }
        }
        public async Task<bool> UpdateCartAsync(Order loggedUserOpenOrder, IEnumerable<int> orderDetailsQuantities)
        {
            try
            {
                var isOrderDetailsQuantitiesContainsAnyUnder1Number =
                    orderDetailsQuantities.Any(p => p < 1);

                if (isOrderDetailsQuantitiesContainsAnyUnder1Number)
                    return false;

                for (var i = 0; i < loggedUserOpenOrder.OrdersDetails.Count; i++)
                {
                    var currentOrderDetails =
                        loggedUserOpenOrder
                            .OrdersDetails
                            .ElementAtOrDefault(i);

                    var orderDetailsQuantity =
                        currentOrderDetails
                            .IsOrderDetailsProductSimple ?
                            currentOrderDetails
                                .Product
                                .ProductQuantityInStock :
                            currentOrderDetails
                                .ProductVariation
                                .ProductVariationQuantityInStock;

                    if (orderDetailsQuantity < orderDetailsQuantities.ElementAtOrDefault(i))
                        return false;

                    currentOrderDetails
                            .OrderDetailsQuantity =
                        orderDetailsQuantities
                            .ElementAtOrDefault(i);

                    var totalCurrentOrderDetailsPrice =
                        currentOrderDetails
                            .IsOrderDetailsProductSimple
                            ? currentOrderDetails
                                  .OrderDetailsQuantity *
                              currentOrderDetails
                                  .Product
                                  .ProductPrice
                            : currentOrderDetails
                                  .OrderDetailsQuantity *
                              currentOrderDetails
                                  .ProductVariation
                                  .ProductVariationPrice;

                    currentOrderDetails
                        .OrderDetailsTotalPrice =
                        totalCurrentOrderDetailsPrice;

                    _productRepository
                        .UpdateOrderDetails(currentOrderDetails);
                }

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<CartCheckOutViewModel> GetLoggedUserCartCheckOutAsync()
        {
            var loggedUserOpenOrder =
                await GetLoggedUserOpenOrderAsync();

            if (loggedUserOpenOrder == null || !loggedUserOpenOrder.OrdersDetails.NotNullOrEmpty())
                return null;

            var cartCheckOutViewModel = new CartCheckOutViewModel()
            {
                FirstName = loggedUserOpenOrder.OwnerUser.UserDetails.FirstName,
                LastName = loggedUserOpenOrder.OwnerUser.UserDetails.LastName,
                MobileNumber = loggedUserOpenOrder.OwnerUser.MobileNumber,
                UserProvince = loggedUserOpenOrder.OwnerUser.UserDetails.UserProvince,
                UserCity = loggedUserOpenOrder.OwnerUser.UserDetails.UserCity,
                UserAddress = loggedUserOpenOrder.OwnerUser.UserDetails.UserAddress,
                UserZipCode = loggedUserOpenOrder.OwnerUser.UserDetails.UserZipCode,
                Order = loggedUserOpenOrder
            };

            return cartCheckOutViewModel;
        }
        public async Task<string> PaymentProcessAsync(CartCheckOutViewModel cartCheckOutViewModel)
        {
            try
            {
                var loggedUserOpenOrder =
                    await GetLoggedUserOpenOrderAsync();

                if (loggedUserOpenOrder == null)
                    return null;

                var totalOrderPrice =
                    loggedUserOpenOrder.OrdersDetails.Sum(p => p.OrderDetailsTotalPrice) -
                    loggedUserOpenOrder.Discounts.Sum(p => p.DiscountPrice);

                var callBackUrl =
                    _linkGenerator
                        .GetUriByAction(_accessor.HttpContext
                            , "OrderConfirmation",
                            "Product");

                var idPayRequestModel = new IdPayRequestModel()
                {
                    order_id = loggedUserOpenOrder.OrderId,
                    amount = totalOrderPrice,
                    callback = callBackUrl
                };

                var jsonIdPayRequestModel =
                    JsonConvert
                    .SerializeObject(idPayRequestModel);

                var idPayRequestModelStringContent =
                    new StringContent(jsonIdPayRequestModel, Encoding.UTF8, "application/json");

                idPayRequestModelStringContent
                    .Headers.Add("X-API-KEY", _configuration["IdPayInformation:ApiKey"]);

                idPayRequestModelStringContent
                    .Headers.Add("X-SANDBOX", _configuration["IdPayInformation:IsTest"].ToString()=="True"?"1":"0");

                var postIdPayResult =
                    await _httpClient
                    .PostAsync("https://api.idpay.ir/v1.1/payment", idPayRequestModelStringContent);

                if (postIdPayResult.StatusCode.ToString() == "Created" || totalOrderPrice <= 0)
                {
                    var postIdPayResultContent =
                        postIdPayResult
                        .Content
                        .ReadAsStringAsync();

                    loggedUserOpenOrder
                            .OwnerUser
                            .UserDetails
                            .FirstName =
                        cartCheckOutViewModel
                            .FirstName;

                    loggedUserOpenOrder
                            .OwnerUser
                            .UserDetails
                            .LastName =
                        cartCheckOutViewModel
                            .LastName;

                    loggedUserOpenOrder
                            .OwnerUser
                            .MobileNumber =
                        cartCheckOutViewModel
                            .MobileNumber;

                    loggedUserOpenOrder
                            .OwnerUser
                            .UserDetails
                            .UserProvince =
                        cartCheckOutViewModel
                            .UserProvince;

                    loggedUserOpenOrder
                            .OwnerUser
                            .UserDetails
                            .UserCity =
                        cartCheckOutViewModel
                            .UserCity;

                    loggedUserOpenOrder
                            .OwnerUser
                            .UserDetails
                            .UserAddress =
                        cartCheckOutViewModel
                            .UserAddress;

                    loggedUserOpenOrder
                            .OwnerUser
                            .UserDetails
                            .UserZipCode =
                        cartCheckOutViewModel
                            .UserZipCode;

                    await _accountService
                        .UpdateUserAsync(loggedUserOpenOrder.OwnerUser);

                    loggedUserOpenOrder
                            .OrderNote =
                        cartCheckOutViewModel
                            .OrderNote;

                    _productRepository
                        .UpdateOrder(loggedUserOpenOrder);

                    await _productRepository
                        .SaveAsync();

                    var responseIdPayModel =
                        JsonConvert
                        .DeserializeObject<IdPayResponseModel>(await postIdPayResultContent);

                    var redirectUrl =
                        totalOrderPrice > 0 ?
                    responseIdPayModel.link :
                    callBackUrl;

                    return redirectUrl;
                }

                return null;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return null;
            }
        }
        public async Task<bool> OrderConfirmationAsync(Order loggedUserOpenOrder)
        {
            try
            {
                var totalOrderPrice =
                    loggedUserOpenOrder.OrdersDetails.Sum(p => p.OrderDetailsTotalPrice) -
                    loggedUserOpenOrder.Discounts.Sum(p => p.DiscountPrice);

                var idPayUniqueKey =
                    _accessor
                    .HttpContext
                    .Request
                    .Query["id"]
                    .ToString();

                if (_accessor.HttpContext.Request.Query["status"] != "" &&
                    _accessor.HttpContext.Request.Query["status"].ToString() == "10" &&
                    _accessor.HttpContext.Request.Query["track_id"] != "" &&
                    _accessor.HttpContext.Request.Query["order_id"].ToString() == loggedUserOpenOrder.OrderId.ToString())
                {
                    var idPayConfirmationModel = new IdPayConfirmationModel()
                    {
                        id = idPayUniqueKey,
                        order_id = loggedUserOpenOrder.OrderId.ToString()
                    };

                    var jsonIdPayConfirmationModel =
                        JsonConvert
                        .SerializeObject(idPayConfirmationModel);

                    var idPayConfirmationModelStringContent =
                        new StringContent(jsonIdPayConfirmationModel, Encoding.UTF8, "application/json");
                    
                    idPayConfirmationModelStringContent
                        .Headers.Add("X-API-KEY", _configuration["IdPayInformation:ApiKey"]);

                    idPayConfirmationModelStringContent
                        .Headers.Add("X-SANDBOX", Convert.ToInt32(_configuration["IdPayInformation:IsTest"]).ToString());

                    var postIdPayConfirmationResult =
                        await _httpClient
                        .PostAsync("https://api.idpay.ir/v1.1/payment/verify", idPayConfirmationModelStringContent);

                    if (postIdPayConfirmationResult.StatusCode.ToString() == "OK")
                    {
                        loggedUserOpenOrder
                                .OrderPaymentTime =
                            DateTime.Now;

                        loggedUserOpenOrder
                                .OrderStatus =
                            OrderStatus
                                .AwaitingReview
                                .GetDescription();

                        loggedUserOpenOrder
                            .IsOrderCompleted = true;

                        _productRepository
                            .UpdateOrder(loggedUserOpenOrder);

                        await _productRepository
                            .SaveAsync();

                        var unFinishedOrdersDetails =
                            await _productRepository
                                .GetUnFinishedOrdersDetailsAsync();

                        foreach (var orderDetails in loggedUserOpenOrder.OrdersDetails)
                        {
                            if (orderDetails.IsOrderDetailsProductSimple)
                            {
                                var finalProductQuantity =
                                    orderDetails
                                        .Product
                                        .ProductQuantityInStock -
                                    orderDetails.OrderDetailsQuantity;

                                finalProductQuantity =
                                    finalProductQuantity > 0 ? finalProductQuantity : 0;

                                orderDetails
                                    .Product
                                    .ProductQuantityInStock =
                                    finalProductQuantity;

                                orderDetails
                                        .Product
                                        .ProductSalesCount +=
                                    orderDetails.OrderDetailsQuantity;

                                _productRepository
                                    .UpdateProduct(orderDetails.Product);

                                var simpleUnFinishedOrdersDetails =
                                    unFinishedOrdersDetails
                                        .Where(p => p.IsOrderDetailsProductSimple);

                                foreach (var unFinishedOrdersDetail in simpleUnFinishedOrdersDetails)
                                {
                                    if (unFinishedOrdersDetail
                                            .Product.ProductId ==
                                        orderDetails.Product.ProductId &&
                                        unFinishedOrdersDetail.OrderDetailsQuantity >
                                        finalProductQuantity)
                                    {
                                        if (finalProductQuantity > 0)
                                        {
                                            unFinishedOrdersDetail
                                                    .OrderDetailsQuantity
                                                = finalProductQuantity;

                                            _productRepository
                                                .UpdateOrderDetails(unFinishedOrdersDetail);
                                        }
                                        else
                                        {
                                            _productRepository
                                                .DeleteOrderDetails(unFinishedOrdersDetail);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var finalProductQuantity =
                                    orderDetails
                                        .ProductVariation
                                        .ProductVariationQuantityInStock -
                                    orderDetails.OrderDetailsQuantity;

                                finalProductQuantity =
                                    finalProductQuantity > 0 ? finalProductQuantity : 0;

                                orderDetails
                                    .ProductVariation
                                    .ProductVariationQuantityInStock =
                                    finalProductQuantity;

                                orderDetails
                                        .Product
                                        .ProductSalesCount +=
                                    orderDetails.OrderDetailsQuantity;

                                _productRepository
                                    .UpdateProductVariation(orderDetails.ProductVariation);

                                var diverseUnFinishedOrdersDetails =
                                    unFinishedOrdersDetails
                                        .Where(p => !p.IsOrderDetailsProductSimple);

                                foreach (var unFinishedOrdersDetail in diverseUnFinishedOrdersDetails)
                                {
                                    if (unFinishedOrdersDetail
                                            .ProductVariation.ProductVariationId ==
                                        orderDetails.ProductVariation.ProductVariationId &&
                                        unFinishedOrdersDetail.OrderDetailsQuantity >
                                        finalProductQuantity)
                                    {
                                        if (finalProductQuantity > 0)
                                        {
                                            unFinishedOrdersDetail
                                                    .OrderDetailsQuantity
                                                = finalProductQuantity;

                                            _productRepository
                                                .UpdateOrderDetails(unFinishedOrdersDetail);
                                        }
                                        else
                                        {
                                            _productRepository
                                                .DeleteOrderDetails(unFinishedOrdersDetail);
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var ordersDetails in loggedUserOpenOrder.OrdersDetails)
                        {
                            var orderInvoiceDetails = new OrderInvoiceDetails()
                            {
                                IsInvoiceDetailsProductSimple = ordersDetails.Product.IsSimpleProduct,
                                InvoiceDetailsProductName = ordersDetails.Product.ProductName,
                                InvoiceDetailsTotalPrice = ordersDetails.OrderDetailsTotalPrice,
                                InvoiceDetailsQuantity = ordersDetails.OrderDetailsQuantity,
                                Order = loggedUserOpenOrder
                            };

                            if (!ordersDetails.IsOrderDetailsProductSimple)
                            {
                                orderInvoiceDetails
                                        .InvoiceDetailsProductAttributesNames =
                                    ordersDetails.Product
                                        .ProductAttributesNames;

                                orderInvoiceDetails
                                        .InvoiceDetailsProductVariationValues =
                                    ordersDetails.ProductVariation
                                        .ProductVariationValues;
                            }

                            await _productRepository
                                .AddInvoiceDetailsAsync(orderInvoiceDetails);
                        }

                        loggedUserOpenOrder
                            .OrdersDetails = null;

                        _productRepository
                            .UpdateOrder(loggedUserOpenOrder);

                        await _productRepository
                            .SaveAsync();

                        return true;
                    }
                }
                else if (totalOrderPrice <= 0)
                {
                    loggedUserOpenOrder
                            .OrderPaymentTime =
                        DateTime.Now;

                    loggedUserOpenOrder
                            .OrderStatus =
                        OrderStatus
                            .AwaitingReview
                            .GetDescription();

                    loggedUserOpenOrder
                        .IsOrderCompleted = true;

                    _productRepository
                        .UpdateOrder(loggedUserOpenOrder);

                    await _productRepository
                        .SaveAsync();

                    var unFinishedOrdersDetails =
                            await _productRepository
                                .GetUnFinishedOrdersDetailsAsync();

                    foreach (var orderDetails in loggedUserOpenOrder.OrdersDetails)
                    {
                        if (orderDetails.IsOrderDetailsProductSimple)
                        {
                            var finalProductQuantity =
                                orderDetails
                                    .Product
                                    .ProductQuantityInStock -
                                orderDetails.OrderDetailsQuantity;

                            finalProductQuantity =
                                finalProductQuantity > 0 ? finalProductQuantity : 0;

                            orderDetails
                                .Product
                                .ProductQuantityInStock =
                                finalProductQuantity;

                            orderDetails
                                    .Product
                                    .ProductSalesCount +=
                                orderDetails.OrderDetailsQuantity;

                            _productRepository
                                .UpdateProduct(orderDetails.Product);

                            var simpleUnFinishedOrdersDetails =
                                unFinishedOrdersDetails
                                    .Where(p => p.IsOrderDetailsProductSimple);

                            foreach (var unFinishedOrdersDetail in simpleUnFinishedOrdersDetails)
                            {
                                if (unFinishedOrdersDetail
                                        .Product.ProductId ==
                                    orderDetails.Product.ProductId &&
                                    unFinishedOrdersDetail.OrderDetailsQuantity >
                                    finalProductQuantity)
                                {
                                    if (finalProductQuantity > 0)
                                    {
                                        unFinishedOrdersDetail
                                                .OrderDetailsQuantity
                                            = finalProductQuantity;

                                        _productRepository
                                            .UpdateOrderDetails(unFinishedOrdersDetail);
                                    }
                                    else
                                    {
                                        _productRepository
                                            .DeleteOrderDetails(unFinishedOrdersDetail);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var finalProductQuantity =
                                orderDetails
                                    .ProductVariation
                                    .ProductVariationQuantityInStock -
                                orderDetails.OrderDetailsQuantity;

                            finalProductQuantity =
                                finalProductQuantity > 0 ? finalProductQuantity : 0;

                            orderDetails
                                .ProductVariation
                                .ProductVariationQuantityInStock =
                                finalProductQuantity;

                            orderDetails
                                    .Product
                                    .ProductSalesCount +=
                                orderDetails.OrderDetailsQuantity;

                            _productRepository
                                .UpdateProductVariation(orderDetails.ProductVariation);

                            var diverseUnFinishedOrdersDetails =
                                unFinishedOrdersDetails
                                    .Where(p => !p.IsOrderDetailsProductSimple);

                            foreach (var unFinishedOrdersDetail in diverseUnFinishedOrdersDetails)
                            {
                                if (unFinishedOrdersDetail
                                        .ProductVariation.ProductVariationId ==
                                    orderDetails.ProductVariation.ProductVariationId &&
                                    unFinishedOrdersDetail.OrderDetailsQuantity >
                                    finalProductQuantity)
                                {
                                    if (finalProductQuantity > 0)
                                    {
                                        unFinishedOrdersDetail
                                                .OrderDetailsQuantity
                                            = finalProductQuantity;

                                        _productRepository
                                            .UpdateOrderDetails(unFinishedOrdersDetail);
                                    }
                                    else
                                    {
                                        _productRepository
                                            .DeleteOrderDetails(unFinishedOrdersDetail);
                                    }
                                }
                            }
                        }
                    }

                    foreach (var ordersDetails in loggedUserOpenOrder.OrdersDetails)
                    {
                        var orderInvoiceDetails = new OrderInvoiceDetails()
                        {
                            IsInvoiceDetailsProductSimple = ordersDetails.Product.IsSimpleProduct,
                            InvoiceDetailsProductName = ordersDetails.Product.ProductName,
                            InvoiceDetailsTotalPrice = ordersDetails.OrderDetailsTotalPrice,
                            InvoiceDetailsQuantity = ordersDetails.OrderDetailsQuantity,
                            Order = loggedUserOpenOrder
                        };

                        if (!ordersDetails.IsOrderDetailsProductSimple)
                        {
                            orderInvoiceDetails
                                    .InvoiceDetailsProductAttributesNames =
                                ordersDetails.Product
                                    .ProductAttributesNames;

                            orderInvoiceDetails
                                    .InvoiceDetailsProductVariationValues =
                                ordersDetails.ProductVariation
                                    .ProductVariationValues;
                        }

                        await _productRepository
                            .AddInvoiceDetailsAsync(orderInvoiceDetails);
                    }

                    loggedUserOpenOrder
                        .OrdersDetails = null;

                    _productRepository
                        .UpdateOrder(loggedUserOpenOrder);

                    await _productRepository
                        .SaveAsync();

                    return true;
                }

                return false;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<AddDiscountToCartResult> AddDiscountToCartAsync(Order loggedUserOpenOrder, string discountCode)
        {
            try
            {
                var discount =
                    await _productRepository
                        .GetDiscountByCodeAsync(discountCode);

                if (discount == null || discount.IsTrash)
                    return AddDiscountToCartResult.Failed;

                var doesLoggedUserOpenOrderHasThisDiscount =
                    loggedUserOpenOrder
                        .Discounts
                        .Contains(discount);

                if (doesLoggedUserOpenOrderHasThisDiscount)
                    return AddDiscountToCartResult.AlreadyApplied;

                loggedUserOpenOrder
                    .Discounts.Add(discount);

                _productRepository.UpdateOrder(loggedUserOpenOrder);

                await _productRepository
                    .SaveAsync();

                return AddDiscountToCartResult.Successful;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return AddDiscountToCartResult.Failed;
            }
        }
        public async Task<IEnumerable<Discount>> GetDiscountsAsync()
        {
            var discounts =
                await _productRepository
                    .GetDiscountsAsync();

            return discounts;
        }
        public async Task<Discount> GetDiscountAsync(int discountId)
        {
            var discount =
                await _productRepository
                    .GetDiscountAsync(discountId);

            return discount;
        }
        public async Task<bool> MoveDiscountToTrashAsync(int discountId)
        {
            try
            {
                var discount =
                    await GetDiscountAsync(discountId);

                if (discount == null)
                    return false;

                var doesDiscountHasAnyFinishedOrder =
                    discount
                        .Orders
                        .Any(p => p.IsOrderCompleted);

                if (!doesDiscountHasAnyFinishedOrder)
                {
                    _productRepository
                        .DeleteDiscount(discount);
                }
                else
                {
                    var unfinishedDiscountOrders =
                        discount.Orders.Where(p => !p.IsOrderCompleted);

                    for (int i = unfinishedDiscountOrders.Count() - 1; i >= 0; i--)
                    {
                        discount
                            .Orders
                            .Remove(unfinishedDiscountOrders.ElementAtOrDefault(i));
                    }

                    discount.IsTrash = true;

                    _productRepository
                        .UpdateDiscount(discount);
                }

                await _productRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<AddUpdateDiscountResult> AddDiscountAsync(Discount discount)
        {
            try
            {
                var discounts =
                    await GetDiscountsAsync();

                var isDiscountCodeExistForAdd =
                    discounts
                        .Any(p => p.DiscountCode == discount.DiscountCode);

                if (isDiscountCodeExistForAdd)
                    return AddUpdateDiscountResult.DiscountCodeExist;

                await _productRepository
                    .AddDiscountAsync(discount);

                await _productRepository
                    .SaveAsync();

                return AddUpdateDiscountResult.Successful;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return AddUpdateDiscountResult.Failed;
            }
        }
        public async Task<AddUpdateDiscountResult> UpdateDiscountAsync(Discount discount)
        {
            try
            {
                var currentDiscount =
                    await GetDiscountAsync(discount.DiscountId);

                if (currentDiscount.DiscountCode != discount.DiscountCode)
                {
                    var isDiscountCodeExistForEdit =
                        await _productRepository
                            .IsDiscountCodeExistAsync(discount.DiscountCode);

                    if (isDiscountCodeExistForEdit)
                        return AddUpdateDiscountResult.DiscountCodeExist;

                    currentDiscount.DiscountCode = discount.DiscountCode;
                }

                currentDiscount.DiscountPrice = discount.DiscountPrice;

                _productRepository.UpdateDiscount(currentDiscount);

                await _productRepository
                    .SaveAsync();

                return AddUpdateDiscountResult.Successful;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return AddUpdateDiscountResult.Failed;
            }
        }

        //End Order Section

        //Utilities Methods

        private async Task<bool> DeleteCascadeCategoryByIdAsync(int categoryId)
        {
            try
            {
                var category =
                    await GetCategoryAsync(categoryId);

                if (category == null)
                    return false;

                var allCategories =
                    await _productRepository
                        .GetCategoriesAsync();

                var childrenCategories =
                    allCategories
                        .Where(p => p.ParentCategory?.CategoryId == categoryId);

                foreach (var childrenCategory in childrenCategories)
                {
                    DeleteChildrenCategories(childrenCategory);

                    _productRepository
                        .DeleteCategory(childrenCategory);
                }

                void DeleteChildrenCategories(Category childrenCategory)
                {
                    var subCategories =
                        allCategories
                            .Where(p => p.ParentCategory?.CategoryId == childrenCategory.CategoryId);

                    foreach (var subCategory in subCategories)
                    {
                        DeleteChildrenCategories(subCategory);

                        _productRepository
                            .DeleteCategory(subCategory);
                    }
                }

                _productRepository
                    .DeleteCategory(category);

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private async Task<bool> AddProductCategoriesAsync(Product product, IEnumerable<int> productCategoriesId)
        {
            try
            {
                foreach (var productCategoryId in productCategoriesId)
                {
                    var productCategory =
                        await GetCategoryAsync(productCategoryId);

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
        private async Task<bool> UpdateProductCategoriesAsync(Product product, IEnumerable<int> productCategoriesId)
        {
            try
            {
                product.Categories
                    .Clear();

                await
                    AddProductCategoriesAsync(product, productCategoriesId);

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private async Task<bool> AddProductsInformationAsync(Product product, IEnumerable<string> informationsNames, IEnumerable<string> informationsValues)
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

                        await _productRepository
                            .AddProductInformationAsync(productInformations);
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
                    _productRepository
                        .DeleteProductInformation(productInformation);
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private async Task<bool> UpdateProductsInformationAsync(Product product, IEnumerable<string> informationsNames, IEnumerable<string> informationsValues)
        {
            try
            {
                if (product.Informations.Any())
                    DeleteProductInformation(product);

                await AddProductsInformationAsync(product, informationsNames, informationsValues);

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private async Task<bool> AddProductAttributesAsync(Product product, AddEditProductViewModel productViewModel)
        {
            try
            {
                product.ProductAttributesNames =
                    productViewModel
                        .AttributesNames.ToList();

                product.ProductAttributesValues =
                    productViewModel
                        .AttributesValues.ToList();

                await _productRepository
                    .SaveAsync();

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

                    await _productRepository
                        .AddProductVariationAsync(productVariation);

                    await _productRepository
                        .SaveAsync();
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private async Task<bool> AddProductAttributesForUpdateAsync(Product product, AddEditProductViewModel productViewModel)
        {
            try
            {
                product.ProductAttributesNames =
                    productViewModel
                        .AttributesNames.ToList();

                product.ProductAttributesValues =
                    productViewModel
                        .AttributesValues.ToList();

                _productRepository
                    .UpdateProduct(product);

                await _productRepository
                    .SaveAsync();

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

                    await _productRepository
                        .AddProductVariationAsync(productVariation);

                    _productRepository
                        .UpdateProduct(product);

                    await _productRepository
                        .SaveAsync();
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        private async Task<bool> DeleteProductAttributesAsync(Product product)
        {
            try
            {
                //All Finished OrderDetails Will Remove After Order Confirmation
                var unFinishedProductOrdersDetails =
                    await _productRepository
                        .GetOrdersDetailsByProductIdAsync(product.ProductId);

                foreach (var unfinishedProductOrderDetails in unFinishedProductOrdersDetails)
                {
                    _productRepository
                        .DeleteOrderDetails(unfinishedProductOrderDetails);
                }

                product.ProductAttributesNames = null;
                product.ProductAttributesValues = null;

                foreach (var productVariation in product.ProductVariations)
                {
                    _productRepository
                        .DeleteProductVariation(productVariation);
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
