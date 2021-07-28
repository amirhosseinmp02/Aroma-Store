using System.Collections.Generic;
using Aroma_Shop.Domain.Models.FileModels;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IProductRepository : IGeneralRepository
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetAvailableProducts();
        Product GetProduct(int productId);
        void AddProduct(Product product);
        void AddProductInformation(ProductInformation productInformation);
        void DeleteProductInformation(ProductInformation productInformation);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        void AddProductVariation(ProductVariation productVariation);
        void UpdateProductVariation(ProductVariation productVariation);
        void DeleteProductVariation(ProductVariation productVariation);
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        void AddCategory(Category category);
        void DeleteCategory(Category category);
        void DeleteCategoryById(int categoryId);
        void UpdateCategory(Category category);

        //Start Order Section

        IEnumerable<Order> GetOrders();
        IEnumerable<Order> GetUserOrders(string userId);
        Order GetOrder(int orderId);
        Order GetOrderWithDetails(int orderId);
        Order GetUserOpenOrder(string userId);
        Order GetUserOrder(string userId, int orderId);
        Order GetUserOrderByEmail(string userEmail, int orderId);
        int GetUnSeenOrdersCount();
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
        IEnumerable<OrderDetails> GetUnFinishedOrdersDetails();
        IEnumerable<OrderDetails> GetOrdersDetailsByProductId(int productId);
        OrderDetails GetOrderDetails(int orderDetailsId);
        int GetUserOpenOrderDetailsCount(string userId);
        void AddOrderDetails(OrderDetails orderDetails);
        void UpdateOrderDetails(OrderDetails orderDetails);
        void DeleteOrderDetails(OrderDetails orderDetails);
        void AddInvoiceDetails(OrderInvoiceDetails invoiceDetails);              
        IEnumerable<Discount> GetDiscounts();
        Discount GetDiscount(int discountId);
        Discount GetDiscountByCode(string discountCode);
        void AddDiscount(Discount discount);
        void UpdateDiscount(Discount discount);
        void DeleteDiscount(Discount discount);

        //End Order Section
    }
}
