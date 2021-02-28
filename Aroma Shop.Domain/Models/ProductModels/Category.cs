using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class Category
    {
        public Category()
        {
            ChildrenCategories = new List<Category>();
            Products = new List<Product>();
        }

        [Key]
        public int CategoryId { get; set; }
        [MaxLength(100, ErrorMessage = "حداکثر 100 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام دسته را وارد نمایید")]
        public string CategoryName { get; set; }

        //Navigations Proterties

        public Category ParentCategory { get; set; }
        public ICollection<Category> ChildrenCategories { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
