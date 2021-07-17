using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Aroma_Shop.Domain.Models.FileModels;
using Aroma_Shop.Domain.Models.MediaModels;

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
            ProductVariations = new List<ProductVariation>();
        }

        [Key]
        public int ProductId { get; set; }
        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام محصول را وارد نمایید")]
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        [MaxLength(1000, ErrorMessage = "حداکثر 1000 کارکتر مجاز می باشد")]
        public string ProductShortDescription { get; set; }
        [Required]
        public bool IsSimpleProduct { get; set; }
        [Required(ErrorMessage = "لطفا قیمت محصول را وارد نمایید")]
        public int ProductPrice { get; set; }
        [Required(ErrorMessage = "لطفا تعداد محصول را وارد نمایید")]
        public int ProductQuantityInStock { get; set; }
        public int ProductHits { get; set; }

        public DateTime RegistrationTime { get; set; }

        //Navigations Properties

        public ICollection<Category> Categories { get; set; }
        public ICollection<ProductInformation> Informations { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Image> Images { get; set; }

        //Attribute Section

        public ICollection<string> ProductAttributesNames { get; set; }
        public ICollection<string> ProductAttributesValues { get; set; }
        public ICollection<ProductVariation> ProductVariations { get; set; }

        //End Of Attribute Section

        public ICollection<OrderDetails> OrdersDetails { get; set; }    
        public ICollection<CustomIdentityUser> InterestedUsers { get; set; }
    }
}
