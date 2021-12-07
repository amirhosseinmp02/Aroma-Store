using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.BlogModels
{
    public class BlogCategory
    {
        [Key]
        public int BlogCategoryId { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام دسته را وارد نمایید")]
        public string BlogCategoryName { get; set; }
        public string BlogCategoryDescription { get; set; }

        //Navigations Proterties

        public BlogCategory ParentBlogCategory { get; set; }
        public ICollection<BlogCategory> ChildrenBlogCategory { get; set; }
        public Blog Blogs { get; set; }
    }
}
