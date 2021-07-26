using System;
using System.Collections.Generic;
using System.Text;

namespace Aroma_Shop.Application.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<Domain.Models.MediaModels.Banner> Banners { get; set; }    
        public IEnumerable<Domain.Models.ProductModels.Product> Products { get; set; }  
    }
}
