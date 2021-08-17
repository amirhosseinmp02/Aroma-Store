using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.FileModels;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.EntityFrameworkCore;

namespace Aroma_Shop.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var products =
                await _context
                .Products
                .Include(p => p.Categories)
                .Include(p => p.Images)
                .Include(p => p.ProductVariations)
                .AsSplitQuery()
                .ToListAsync();

            return products;
        }
        public async Task<IEnumerable<Product>> GetAvailableProductsAsync()
        {
            var availableProducts =
                await _context
                    .Products
                    .Where(p => p.IsSimpleProduct ?
                    p.ProductQuantityInStock > 0 :
                    p.ProductVariations.Any(p => p.ProductVariationQuantityInStock > 0))
                    .Include(p => p.Categories)
                    .Include(p => p.Images)
                    .Include(p => p.ProductVariations)
                    .AsSplitQuery()
                    .ToListAsync();

            return availableProducts;
        }
        public async Task<Product> GetProductAsync(int productId)
        {
            var product =
                await _context
                    .Products
                    .FindAsync(productId);

            return product;
        }
        public async Task<Product> GetProductWithDetailsAsync(int productId)
        {
            var product =
                await _context.Products
                .Include(p => p.Comments)
                .ThenInclude(p => p.User)
                .SingleOrDefaultAsync(p => p.ProductId == productId);

            if (product != null)
            {
                await _context
                    .Entry(product)
                    .Collection(p => p.Categories)
                    .LoadAsync();

                await _context
                    .Entry(product)
                    .Collection(p => p.Informations)
                    .LoadAsync();

                await _context
                    .Entry(product)
                    .Collection(p => p.Images)
                    .LoadAsync();

                await _context
                    .Entry(product)
                    .Collection(p => p.ProductVariations)
                    .LoadAsync();
            }

            return product;
        }
        public async Task<int> GetProductsCountAsync()
        {
            var productsCount =
                await _context
                    .Products
                    .CountAsync();

            return productsCount;
        }
        public async Task<bool> IsProductExist(int productId)
        {
            var isProductExist =
                await _context
                .Products
                .AnyAsync(p => p.ProductId == productId);

            return isProductExist;
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categories =
                await _context
                .Categories
                .Include(p => p.ParentCategory)
                .Include(p => p.Products)
                .AsSplitQuery()
                .ToListAsync();

            return categories;
        }
        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            var category =
                await _context
                .Categories
                .Include(p => p.ParentCategory)
                .SingleOrDefaultAsync(p => p.CategoryId == categoryId);

            return category;
        }
        public async Task AddProductAsync(Product product)
        {
            await _context
                .AddAsync(product);
        }
        public async Task AddProductInformationAsync(ProductInformation productInformation)
        {
            await _context
                .AddAsync(productInformation);
        }
        public void DeleteProductInformation(ProductInformation productInformation)
        {
            _context
                .Remove(productInformation);
        }
        public void UpdateProduct(Product product)
        {
            _context
                .Update(product);
        }
        public void DeleteProduct(Product product)
        {
            _context
                .Remove(product);
        }
        public async Task AddProductVariationAsync(ProductVariation productVariation)
        {
            await _context
                .AddAsync(productVariation);
        }
        public void UpdateProductVariation(ProductVariation productVariation)
        {
            _context
                .Update(productVariation);
        }
        public void DeleteProductVariation(ProductVariation productVariation)
        {
            _context
                .Remove(productVariation);
        }
        public async Task AddCategoryAsync(Category category)
        {
            await _context
                .AddAsync(category);
        }
        public void UpdateCategory(Category category)
        {
            _context
                .Update(category);
        }
        public void DeleteCategory(Category category)
        {
            _context
                .Remove(category);
        }
        public async void DeleteCategoryById(int categoryId)
        {
            var category =
                await GetCategoryAsync(categoryId);

            _context
                .Remove(category);
        }

        //Start Order Section 

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            var orders =
                await _context
                    .Orders
                    .Include(p => p.OrdersDetails)
                    .Include(p => p.InvoicesDetails)
                    .Include(p => p.Discounts)
                    .Include(p => p.OwnerUser)
                    .ThenInclude(p => p.UserDetails)
                    .ToListAsync();

            return orders;
        }
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            var orders =
                await _context
                    .Orders
                    .Include(p => p.OwnerUser)
                    .ThenInclude(p => p.UserDetails)
                    .Where(p => p.OwnerUser.Id == userId)
                    .Include(p => p.OrdersDetails)
                    .Include(p => p.Discounts)
                    .Include(p => p.InvoicesDetails)
                    .ToListAsync();

            return orders;
        }
        public async Task<Order> GetOrderAsync(int orderId)
        {
            var order =
                await _context
                    .Orders
                    .FindAsync(orderId);

            return order;
        }
        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            var order =
                await _context
                    .Orders
                    .Include(p => p.OrdersDetails)
                    .ThenInclude(p => p.Product)
                    .Include(p => p.OrdersDetails)
                    .ThenInclude(p => p.ProductVariation)
                    .SingleOrDefaultAsync(p => p.OrderId == orderId);

            if (order != null)
            {
                await _context
                    .Entry(order)
                    .Collection(p => p.InvoicesDetails)
                    .LoadAsync();

                await _context
                    .Entry(order)
                    .Collection(p => p.Discounts)
                    .LoadAsync();

                await _context
                    .Entry(order)
                    .Reference(p => p.OwnerUser)
                    .LoadAsync();

                await _context
                    .Entry(order.OwnerUser)
                    .Reference(p => p.UserDetails)
                    .LoadAsync();
            }

            return order;
        }
        public async Task<Order> GetUserOpenOrderAsync(string userId)
        {
            var order =
                await _context
                    .Orders
                    .Include(p => p.OwnerUser)
                    .ThenInclude(p => p.UserDetails)
                    .Include(p => p.OrdersDetails)
                    .ThenInclude(p => p.Product)
                    .ThenInclude(p => p.Images)
                    .Include(p => p.OrdersDetails)
                    .ThenInclude(p => p.ProductVariation)
                    .SingleOrDefaultAsync(p => p.OwnerUser.Id == userId && !p.IsOrderCompleted);

            if (order != null)
            {
                await _context
                    .Entry(order)
                    .Collection(p => p.Discounts)
                    .LoadAsync();
            }

            return order;
        }
        public async Task<Order> GetUserOrderAsync(string userId, int orderId)
        {
            var order =
                await _context
                    .Orders
                    .Include(p => p.OwnerUser)
                    .ThenInclude(p => p.UserDetails)
                    .Include(p => p.OrdersDetails)
                    .ThenInclude(p => p.Product)
                    .Include(p => p.OrdersDetails)
                    .ThenInclude(p => p.ProductVariation)
                    .SingleOrDefaultAsync(p => p.OrderId == orderId && p.OwnerUser.Id == userId);

            if (order != null)
            {
                await _context
                    .Entry(order)
                    .Collection(p => p.InvoicesDetails)
                    .LoadAsync();

                await _context
                    .Entry(order)
                    .Collection(p => p.Discounts)
                    .LoadAsync();
            }

            return order;
        }
        public async Task<Order> GetUserOrderByEmailAsync(string userEmail, int orderId)
        {
            var userOrder =
                await _context
                    .Orders
                    .Include(p => p.OwnerUser)
                    .ThenInclude(p => p.UserDetails)
                    .Include(p => p.OrdersDetails)
                    .ThenInclude(p => p.Product)
                    .Include(p => p.OrdersDetails)
                    .ThenInclude(p => p.ProductVariation)
                    .SingleOrDefaultAsync(p => p.OrderId == orderId);

            if (userOrder != null)
            {
                await _context
                    .Entry(userOrder)
                    .Collection(p => p.InvoicesDetails)
                    .LoadAsync();

                await _context
                    .Entry(userOrder)
                    .Collection(p => p.Discounts)
                    .LoadAsync();
            }

            return userOrder;
        }
        public async Task<int> GetCompletedOrdersCountAsync()
        {
            var completedOrdersCount =
                await _context
                    .Orders
                    .CountAsync(p => p.IsOrderCompleted);

            return completedOrdersCount;
        }
        public async Task<int> GetUnCompletedOrdersCountAsync()
        {
            var unCompletedOrdersCount =
                await _context
                    .Orders
                    .CountAsync(p => !p.IsOrderCompleted);

            return unCompletedOrdersCount;
        }
        public async Task<int> GetUnSeenOrdersCountAsync()
        {
            var unSeenOrdersCount =
                await _context
                    .Orders
                    .CountAsync(p => !p.IsOrderSeen);

            return unSeenOrdersCount;
        }
        public async Task AddOrderAsync(Order order)
        {
            await _context
                .AddAsync(order);
        }
        public void UpdateOrder(Order order)
        {
            _context.Update(order);
        }
        public void DeleteOrder(Order order)
        {
            _context
                .Remove(order);
        }
        public async Task<IEnumerable<OrderDetails>> GetUnFinishedOrdersDetailsAsync()
        {
            var unFinishedOrdersDetails =
                await _context
                    .OrdersDetails
                    .Where(p => !p.Order.IsOrderCompleted)
                    .Include(p => p.Product)
                    .Include(p => p.ProductVariation)
                    .ToListAsync();

            return unFinishedOrdersDetails;
        }
        public async Task<IEnumerable<OrderDetails>> GetOrdersDetailsByProductIdAsync(int productId)
        {
            var orderDetails =
                await _context
                    .OrdersDetails
                    .Where(p => p.Product.ProductId == productId)
                    .Include(p => p.Order)
                    .Include(p => p.ProductVariation)
                    .ToListAsync();

            return orderDetails;
        }
        public async Task<OrderDetails> GetOrderDetailsAsync(int orderDetailsId)
        {
            var orderDetails =
                await _context
                    .OrdersDetails
                    .FindAsync(orderDetailsId);

            return orderDetails;
        }
        public async Task<int> GetUserOpenOrderDetailsCountAsync(string userId)
        {
            var userOpenOrderDetailsCount =
                await _context
                    .OrdersDetails
                    .CountAsync(p => p.Order.OwnerUser.Id == userId && !p.Order.IsOrderCompleted);

            return userOpenOrderDetailsCount;
        }
        public async Task AddOrderDetailsAsync(OrderDetails orderDetails)
        {
            await _context
                .AddAsync(orderDetails);
        }
        public void UpdateOrderDetails(OrderDetails orderDetails)
        {
            _context
                .Update(orderDetails);
        }
        public void DeleteOrderDetails(OrderDetails orderDetails)
        {
            _context
                .Remove(orderDetails);
        }
        public async Task AddInvoiceDetailsAsync(OrderInvoiceDetails invoiceDetails)
        {
            await _context
                .AddAsync(invoiceDetails);
        }
        public async Task<IEnumerable<Discount>> GetDiscountsAsync()
        {
            var discounts =
                await _context
                    .Discounts
                    .ToListAsync();

            return discounts;
        }
        public async Task<Discount> GetDiscountAsync(int discountId)
        {
            var discount =
                await _context
                    .Discounts
                    .Include(p => p.Orders)
                    .SingleOrDefaultAsync(p => p.DiscountId == discountId);

            return discount;
        }
        public async Task<Discount> GetDiscountByCodeAsync(string discountCode)
        {
            var discount =
                await _context
                    .Discounts
                    .SingleOrDefaultAsync(p => p.DiscountCode == discountCode);

            return discount;
        }
        public async Task<bool> IsDiscountCodeExistAsync(string discountCode)
        {
            var isDiscountCodeExist =
                await _context
                    .Discounts
                    .AnyAsync(p => p.DiscountCode == discountCode);

            return isDiscountCodeExist;
        }
        public async Task AddDiscountAsync(Discount discount)
        {
            await _context
                .AddAsync(discount);
        }
        public void UpdateDiscount(Discount discount)
        {
            _context
                .Update(discount);
        }
        public void DeleteDiscount(Discount discount)
        {
            _context
                .Remove(discount);
        }

        //End Order Section

        public async Task SaveAsync()
        {
            await _context
                .SaveChangesAsync();
        }
    }
}
