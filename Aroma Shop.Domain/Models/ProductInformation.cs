using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models
{
    public class ProductInformation
    {
        [Key]
        public int ProductInformationId { get; set; }
        [MaxLength(100, ErrorMessage = "حداکثر 100 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام مشخصه را وارد نمایید")]
        public string Name { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا مقدار مشخصه را وارد نمایید")]
        public string Value { get; set; }
    }
}
