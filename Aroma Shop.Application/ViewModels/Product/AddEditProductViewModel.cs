using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class AddEditProductViewModel
    {
        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام محصول را وارد نمایید")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "لطفا قیمت محصول را وارد نمایید")]
        public double ProductPrice { get; set; }
        [Required(ErrorMessage = "لطفا تعداد محصول را وارد نمایید")]
        public int ProductQuantityInStock { get; set; }
        [MaxLength(10000, ErrorMessage = "حداکثر 10000 کارکتر مجاز می باشد")]
        public string ProductDescription { get; set; }

        public IFormCollection ProductImages { get; set; }

    }
}
