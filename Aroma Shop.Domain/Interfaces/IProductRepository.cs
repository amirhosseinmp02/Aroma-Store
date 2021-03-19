using System.Collections.Generic;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int productId);
        void AddProduct(Product product);
        void AddProductInformations(ProductInformation productInformation);
        void UpdateProduct(Product product);

        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        void AddCategory(Category category);
        void DeleteCategory(int categoryId);
        void UpdateCategory(Category category);
        IEnumerable<Image> GetImages();
        void AddImage(Image image);
        void DeleteImage(Image image);
        void Save();
    }
}
