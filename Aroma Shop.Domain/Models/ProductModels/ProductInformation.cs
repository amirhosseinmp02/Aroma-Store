using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class ProductInformation
    {
        [Key]
        public int ProductInformationId { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام مشخصه را وارد نمایید")]
        public string Name { get; set; }
        [MaxLength(300, ErrorMessage = "حداکثر 300 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا مقدار مشخصه را وارد نمایید")]
        public string Value { get; set; }

        //Navigations Proterties

        public Product Product { get; set; }
    }
}
