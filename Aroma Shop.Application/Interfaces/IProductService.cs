﻿using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int productId);
    }
}
