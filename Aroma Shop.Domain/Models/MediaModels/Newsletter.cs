using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.MediaModels
{
    public class Newsletter
    {
        [Key]
        public int NewsletterId { get; set; }
        [EmailAddress(ErrorMessage = "فیلد وارد شده ایمیل نمی باشد")]
        [MaxLength(256, ErrorMessage = "حداکثر 256 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا آدرس ایمیل خود را وارد نمایید")]
        public string CustomerEmail { get; set; }
    }
}
