using System.Collections.Generic;
using System.Threading.Tasks;
using Aroma_Shop.Domain.Models.FileModels;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IProductRepository : IGeneralRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<IEnumerable<Product>> GetAvailableProductsAsync();
        Task<Product> GetProductAsync(int productId);
        Task<Product> GetProductWithDetailsAsync(int productId);
        Task<int> GetProductsCountAsync();
        Task<bool> IsProductExist(int productId);
        Task AddProductAsync(Product product);
        Task AddProductInformationAsync(ProductInformation productInformation);
        void DeleteProductInformation(ProductInformation productInformation);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        Task AddProductVariationAsync(ProductVariation productVariation);
        void UpdateProductVariation(ProductVariation productVariation);
        void DeleteProductVariation(ProductVariation productVariation);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(int categoryId);
        Task AddCategoryAsync(Category category);
        void DeleteCategory(Category category);
        void DeleteCategoryById(int categoryId);
        void UpdateCategory(Category category);

        //Start Order Section

        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order> GetOrderAsync(int orderId);
        Task<Order> GetOrderWithDetailsAsync(int orderId);
        Task<Order> GetUserOpenOrderAsync(string userId);
        Task<Order> GetUserOrderAsync(string userId, int orderId);
        Task<Order> GetUserOrderByEmailAsync(string userEmail, int orderId);
        Task<int> GetCompletedOrdersCountAsync();
        Task<int> GetUnCompletedOrdersCountAsync();
        Task<int> GetUnSeenOrdersCountAsync();
        Task AddOrderAsync(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
        Task<IEnumerable<OrderDetails>> GetUnFinishedOrdersDetailsAsync();
        Task<IEnumerable<OrderDetails>> GetOrdersDetailsByProductIdAsync(int productId);
        Task<OrderDetails> GetOrderDetailsAsync(int orderDetailsId);
        Task<int> GetUserOpenOrderDetailsCountAsync(string userId);
        Task AddOrderDetailsAsync(OrderDetails orderDetails);
        void UpdateOrderDetails(OrderDetails orderDetails);
        void DeleteOrderDetails(OrderDetails orderDetails);
        Task AddInvoiceDetailsAsync(OrderInvoiceDetails invoiceDetails);              
        Task<IEnumerable<Discount>> GetDiscountsAsync();
        Task<Discount> GetDiscountAsync(int discountId);
        Task<Discount> GetDiscountByCodeAsync(string discountCode);
        Task<bool> IsDiscountCodeExistAsync(string discountCode);
        Task AddDiscountAsync(Discount discount);
        void UpdateDiscount(Discount discount);
        void DeleteDiscount(Discount discount);

        //End Order Section
    }
}
