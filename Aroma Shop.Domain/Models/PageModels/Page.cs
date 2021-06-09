using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.PageModels
{
    public class Page
    {
        [Key]
        public int PageId { get; set; }
        [MaxLength(300, ErrorMessage = "حداکثر 300 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا عنوان صفحه را وارد نمایید")]
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        [MaxLength(300, ErrorMessage = "حداکثر 500 کارکتر مجاز می باشد")]
        [Required]
        public string PageLink { get; set; }    
    }
}
