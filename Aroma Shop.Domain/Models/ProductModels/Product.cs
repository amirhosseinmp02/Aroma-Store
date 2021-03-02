using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class Product
    {
        public Product()
        {
            Categories = new List<Category>();
            Informations = new List<ProductInformation>();
            Comments = new List<Comment>();
            Images = new List<Image>();
        }

        [Key]
        public int ProductId { get; set; }
        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام محصول را وارد نمایید")]
        public string ProductName { get; set; }
        [MaxLength(10000, ErrorMessage = "حداکثر 10000 کارکتر مجاز می باشد")]
        public string ProductDescription { get; set; }
        [Required(ErrorMessage = "لطفا قیمت محصول را وارد نمایید")]
        public double ProductPrice { get; set; }
        [Required(ErrorMessage = "لطفا تعداد محصول را وارد نمایید")]
        public int ProductQuantityInStock { get; set; }

        //Navigations Properties

        public ICollection<Category> Categories { get; set; }
        public ICollection<ProductInformation> Informations { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Image> Images { get; set; }
    }
}
