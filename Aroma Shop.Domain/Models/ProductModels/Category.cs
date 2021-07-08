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
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام دسته را وارد نمایید")]
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        //Navigations Proterties

        public Category ParentCategory { get; set; }
        public ICollection<Category> ChildrenCategories { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
