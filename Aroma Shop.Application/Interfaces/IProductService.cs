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
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetAvailableProducts();
        Product GetProduct(int productId);
        int GetProductsCount();
        bool AddProduct(AddEditProductViewModel productViewModel);
        bool UpdateProduct(AddEditProductViewModel productViewModel);
        bool DeleteProduct(Product product);
        bool DeleteProductById(int productId);
        bool AddHitsToProduct(Product product);
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        bool AddCategory(AddEditCategoryViewModel categoryViewModel);
        bool UpdateCategory(AddEditCategoryViewModel categoryViewModel);
        bool DeleteCategoryById(int categoryId);
        IEnumerable<CategoryTreeView> GetCategoriesTreeViews();       
        IEnumerable<SelectListItem> GetCategoriesTreeViewForAdd();
        IEnumerable<SelectListItem> GetCategoriesTreeViewForEdit(Category selfCategory);
        Task<bool> IsProductInLoggedUserFavoriteProducts(int favoriteProductId);
        Task<bool> AddProductByIdToLoggedUserFavoriteProducts(int favoriteProductId);
        Task<bool> RemoveProductByIdFromLoggedUserFavoriteProducts(int favoriteProductId);
        Task<IEnumerable<Product>> GetLoggedUserFavoriteProducts();

        //Start Order Section

        IEnumerable<Order> GetOrders();
        IEnumerable<OrdersViewModel> GetOrdersListView();       
        IEnumerable<OrdersViewModel> GetLoggedUserOrders();
        OrderViewModel GetOrderForEdit(int orderId);
        Order GetLoggedUserOpenOrder();
        OrderViewModel OrderTrackingByUserEmail(string userEmail, int orderId);
        OrderViewModel GetConfirmedOrderInvoice(Order confirmedOrder);
        OrderViewModel GetLoggedUserOrderInvoice(int orderId);
        int GetCompletedOrdersCount();
        int GetUnCompletedOrdersCount();
        int GetUnSeenOrdersCount();
        bool UpdateOrder(Order order);
        bool SetOrderAsSeen(Order order);
        bool DeleteOrderById(int orderId);
        int GetLoggedUserOpenOrderDetailsCount();
        bool DeleteOrderDetailsById(int orderDetailsId);
        Task<AddProductToCartResult> AddProductToCart(Product product, int requestedQuantity = 1, int productVariationId = -1);
        bool UpdateCart(Order loggedUserOpenOrder, IEnumerable<int> orderDetailsQuantities);
        CartCheckOutViewModel GetLoggedUserCartCheckOut();
        Task<string> PaymentProcess(CartCheckOutViewModel cartCheckOutViewModel);
        Task<bool> OrderConfirmation(Order loggedUserOpenOrder);
        Task<AddDiscountToCartResult> AddDiscountToCart(Order loggedUserOpenOrder, string discountCode);
        IEnumerable<Discount> GetDiscounts();
        Discount GetDiscount(int discountId);
        bool MoveDiscountToTrash(int discountId);
        AddUpdateDiscountResult AddDiscount(Discount discount);
        AddUpdateDiscountResult UpdateDiscount(Discount discount);

        //End Order Section
    }
}
