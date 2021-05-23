using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class ProductViewModel
    {
        public Domain.Models.ProductModels.Product Product { get; set; }
        [Required(ErrorMessage = "لطفا نظر خود را وارد کنید")]
        [MaxLength(10000, ErrorMessage = "حداکثر 10000 کارکتر مجاز می باشد")]
        public string CommentMessage { get; set; }
    }
}
