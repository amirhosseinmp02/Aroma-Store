using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class Discount
    {
        [Key]
        public int DiscountId { get; set; }
        [Required(ErrorMessage = "لطفا کد تخفیف را وارد نمایید")]
        [MinLength(5,ErrorMessage = "حداقل 5 کارکتر مجاز می باشد")]
        [MaxLength(15, ErrorMessage = "حداکثر 15 کارکتر مجاز می باشد")]
        public string DiscountCode { get; set; }
        [Required(ErrorMessage = "لطفا میزان تخفیف را وارد نمایید")]
        public int DiscountPrice { get; set; }
        public bool IsTrash { get; set; }   

        //Navigations Properties

        public ICollection<Order> Orders { get; set; }  
    }
}
