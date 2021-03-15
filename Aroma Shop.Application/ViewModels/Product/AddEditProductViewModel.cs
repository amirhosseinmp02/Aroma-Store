using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomValidationAttribute;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class AddEditProductViewModel
    {
        public AddEditProductViewModel()
        {
            ProductCategoriesId = new List<int>();
            ProductImages = new List<IFormFile>();
            InformationsNames = new List<string>();
            InformationsValues = new List<string>();
        }

        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام محصول را وارد نمایید")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "لطفا قیمت محصول را وارد نمایید")]
        [Range(0,999999999,ErrorMessage = "حداقل مقدار 0 می باشد")] 
        public double ProductPrice { get; set; }
        [Required(ErrorMessage = "لطفا تعداد محصول را وارد نمایید")]
        [Range(0, 999999999, ErrorMessage = "حداقل مقدار 0 می باشد")]
        public int ProductQuantityInStock { get; set; }
        [MaxLength(10000, ErrorMessage = "حداکثر 10000 کارکتر مجاز می باشد")]
        public string ProductDescription { get; set; }
        public string CategoriesTreeViewNodesJson { get; set; }

        [MaxFilesCount(6)]
        [MaxFileSize(4194304)]
        [AllowedExtensions(new string[]{".png",".jpg",".jpeg"})]
        public IEnumerable<IFormFile> ProductImages { get; set; }
        public IEnumerable<int> ProductCategoriesId { get; set; }

        public IEnumerable<string> InformationsNames { get; set; }
        public IEnumerable<string> InformationsValues { get; set; }

    }
}
