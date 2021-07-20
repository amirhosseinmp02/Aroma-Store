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
        Product GetProduct(int productId);
        bool AddProduct(AddEditProductViewModel productViewModel);
        bool UpdateProduct(AddEditProductViewModel productViewModel);
        bool DeleteProduct(Product product);
        bool DeleteProductById(int productId);
        bool AddHitsToProduct(Product product);
        Task<AddProductToCartResult> AddProductToCart(Product product, int requestedQuantity = 1, int productVariationId = -1);
        bool DeleteOrderDetailsById(int orderDetailsId);
        IEnumerable<Discount> GetDiscounts();
        Discount GetDiscount(int discountId);
        bool MoveDiscountToTrash(int discountId);
        AddUpdateDiscountResult AddDiscount(Discount discount);
        AddUpdateDiscountResult UpdateDiscount(Discount discount);
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        bool AddCategory(AddEditCategoryViewModel categoryViewModel);
        bool UpdateCategory(AddEditCategoryViewModel categoryViewModel);
        bool DeleteCategoryById(int categoryId);
        IEnumerable<SelectListItem> GetCategoriesTreeView();
        IEnumerable<SelectListItem> GetCategoriesTreeViewForEdit(Category selfCategory);
        Task<bool> IsProductInLoggedUserFavoriteProducts(int favoriteProductId);
        Task<bool> AddProductByIdToLoggedUserFavoriteProducts(int favoriteProductId);
        Task<bool> RemoveProductByIdFromLoggedUserFavoriteProducts(int favoriteProductId);
        Task<IEnumerable<Product>> GetLoggedUserFavoriteProducts();
    }
}
