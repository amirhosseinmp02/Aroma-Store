using System.Collections.Generic;
using Aroma_Shop.Domain.Models.FileModels;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IProductRepository : IGeneralRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int productId);
        void AddProduct(Product product);
        void AddProductInformation(ProductInformation productInformation);
        void DeleteProductInformation(ProductInformation productInformation);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        void AddProductVariation(ProductVariation productVariation);
        void UpdateProductVariation(ProductVariation productVariation);
        void DeleteProductVariation(ProductVariation productVariation);
        IEnumerable<Order> GetOrders();
        int GetUnSeenOrdersCount();
        Order GetOrder(int orderId);
        Order GetOrderForAdmin(int orderId);
        void DeleteOrder(Order order);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        IEnumerable<OrderDetails> GetUnFinishedOrdersDetails(); 
        OrderDetails GetOrderDetails(int orderDetailsId);
        void AddOrderDetails(OrderDetails orderDetails);
        void UpdateOrderDetails(OrderDetails orderDetails);
        void DeleteOrderDetails(OrderDetails orderDetails);
        IEnumerable<Discount> GetDiscounts();
        Discount GetDiscount(int discountId);
        Discount GetDiscountByCode(string discountCode);
        void AddDiscount(Discount discount);
        void UpdateDiscount(Discount discount);
        void DeleteDiscount(Discount discount);
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        void AddCategory(Category category);
        void DeleteCategory(Category category);
        void DeleteCategoryById(int categoryId);
        void UpdateCategory(Category category);
    }
}
