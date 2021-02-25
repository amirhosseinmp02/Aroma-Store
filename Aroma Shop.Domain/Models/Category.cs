﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [MaxLength(100, ErrorMessage = "حداکثر 100 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام دسته را وارد نمایید")]
        public string CategoryName { get; set; }
    }
}
