using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct();
    }
}
