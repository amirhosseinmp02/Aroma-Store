using System.Collections.Generic;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int productId);

        IEnumerable<Category> GetCategories();
        IEnumerable<Category> GetDeepCategories();
        Category GetCategory(int categoryId);
    }
}
