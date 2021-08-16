using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAvailableProductsAsync();
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(int productId);
        Task<Product> GetProductWithDetailsAsync(int productId);
        Task<int> GetProductsCountAsync();
        Task<bool> AddProductAsync(AddEditProductViewModel productViewModel);
        Task<bool> UpdateProductAsync(AddEditProductViewModel productViewModel);
        Task<bool> DeleteProductAsync(Product product);
        Task<bool> DeleteProductByIdAsync(int productId);
        Task<bool> AddHitsToProductAsync(Product product);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(int categoryId);
        Task<bool> AddCategoryAsync(AddEditCategoryViewModel categoryViewModel);
        Task<bool> UpdateCategoryAsync(AddEditCategoryViewModel categoryViewModel);
        Task<bool> DeleteCategoryByIdAsync(int categoryId);
        Task<IEnumerable<CategoryTreeView>> GetCategoriesTreeViewsAsync();       
        Task<IEnumerable<SelectListItem>> GetCategoriesTreeViewForAddAsync();
        Task<IEnumerable<SelectListItem>> GetCategoriesTreeViewForEditAsync(Category selfCategory);
        Task<bool> AddProductByIdToLoggedUserFavoriteProductsAsync(int favoriteProductId);
        Task<bool> RemoveProductByIdFromLoggedUserFavoriteProductsAsync(int favoriteProductId);
        Task<IEnumerable<Product>> GetLoggedUserFavoriteProductsAsync();

        //Start Order Section

        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<IEnumerable<OrdersViewModel>> GetOrdersListViewAsync();       
        Task<IEnumerable<OrdersViewModel>> GetLoggedUserOrdersAsync();
        Task<OrderViewModel> GetOrderForEditAsync(int orderId);
        Task<Order> GetLoggedUserOpenOrderAsync();
        Task<OrderViewModel> OrderTrackingByUserEmailAsync(string userEmail, int orderId);
        OrderViewModel GetConfirmedOrderInvoiceAsync(Order confirmedOrder);
        Task<OrderViewModel> GetLoggedUserOrderInvoiceAsync(int orderId);
        Task<int> GetCompletedOrdersCountAsync();
        Task<int> GetUnCompletedOrdersCountAsync();
        Task<int> GetUnSeenOrdersCountAsync();
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> SetOrderAsSeenAsync(Order order);
        Task<bool> DeleteOrderByIdAsync(int orderId);
        Task<int> GetLoggedUserOpenOrderDetailsCountAsync();
        Task<bool> DeleteOrderDetailsByIdAsync(int orderDetailsId);
        Task<AddProductToCartResult> AddProductToCartAsync(Product product, int requestedQuantity = 1, int productVariationId = -1);
        Task<bool> UpdateCartAsync(Order loggedUserOpenOrder, IEnumerable<int> orderDetailsQuantities);
        Task<CartCheckOutViewModel> GetLoggedUserCartCheckOutAsync();
        Task<string> PaymentProcessAsync(CartCheckOutViewModel cartCheckOutViewModel);
        Task<bool> OrderConfirmationAsync(Order loggedUserOpenOrder);
        Task<AddDiscountToCartResult> AddDiscountToCartAsync(Order loggedUserOpenOrder, string discountCode);
        Task<IEnumerable<Discount>> GetDiscountsAsync();
        Task<Discount> GetDiscountAsync(int discountId);
        Task<bool> MoveDiscountToTrashAsync(int discountId);
        Task<AddUpdateDiscountResult> AddDiscountAsync(Discount discount);
        Task<AddUpdateDiscountResult> UpdateDiscountAsync(Discount discount);

        //End Order Section
    }
}
