using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Aroma_Shop.Domain.Models.FileModels;
using Aroma_Shop.Domain.Models.MediaModels;

namespace Aroma_Shop.Domain.Models.BlogModels
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا عنوان مطلب را وارد نمایید")]
        public string BlogTitle { get; set; }
        public string BlogDescription { get; set; }
        [MaxLength(1000, ErrorMessage = "حداکثر 1000 کارکتر مجاز می باشد")]
        public string BlogShortDescription { get; set; }
        public DateTime CreateTime { get; set; }

        //Navigations Properties

        public CustomIdentityUser BuilderUser { get; set; }
        public Image BlogImage { get; set; }
        public ICollection<BlogCategory> BlogCategories { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
