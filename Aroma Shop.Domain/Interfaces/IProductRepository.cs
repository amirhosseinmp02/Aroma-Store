using System.Collections.Generic;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int productId);

        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        void AddCategory(Category category);
        void DeleteCategory(int categoryId);
        void Save();
    }
}
