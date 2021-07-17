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
        void DeleteProductVariation(ProductVariation productVariation);
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        void AddCategory(Category category);
        void DeleteCategory(Category category);
        void DeleteCategoryById(int categoryId);
        void UpdateCategory(Category category);
    }
}
