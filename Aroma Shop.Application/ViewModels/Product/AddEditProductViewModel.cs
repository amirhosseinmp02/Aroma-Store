using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomValidationAttribute;
using Aroma_Shop.Domain.Models.FileModels;
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
            ProductImagesFiles = new List<IFormFile>();
            AttributesNames = new List<string>();
            AttributesValues = new List<string>();
            ProductVariationsNames = new List<string>();
            ProductVariationsPrices = new List<int?>();
            ProductVariationsQuantityInStocks = new List<int?>();
            InformationNames = new List<string>();
            InformationValues = new List<string>();
        }

        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام محصول را وارد نمایید")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "لطفا قیمت محصول را وارد نمایید")]
        [Range(0, 999999999, ErrorMessage = "حداقل مقدار 0 می باشد")]
        public int ProductPrice { get; set; }
        [Required(ErrorMessage = "لطفا تعداد محصول را وارد نمایید")]
        [Range(0, 999999999, ErrorMessage = "حداقل مقدار 0 می باشد")]
        public int ProductQuantityInStock { get; set; }
        public string ProductDescription { get; set; }
        [MaxLength(1000, ErrorMessage = "حداکثر 1000 کارکتر مجاز می باشد")]
        public string ProductShortDescription { get; set; }

        [Required(ErrorMessage = "لطفا نوع محصول را انتخاب نمایید")]
        public bool IsSimpleProduct { get; set; } = true;
        [MaxStringsLength(200,ErrorMessage = "حداکثر 200 کارکتر برای نام صفت مجاز می باشد")]
        [NotIncludedInStrings(",", ErrorMessage = "نام صفت نمی تواند شامل کارکتر , باشد")]
        [RequiredStringsIf(nameof(IsSimpleProduct),false, ErrorMessage = "لطفا نام صفت را وارد نمایید")]
        public IEnumerable<string> AttributesNames { get; set; }
        [NotIncludedInStrings("-",ErrorMessage = "مقادیر صفت نمی توانند شامل کارکتر - باشند")]
        [RequiredStringsIf(nameof(IsSimpleProduct), false, ErrorMessage = "لطفا مقادیر صفت را وارد نمایید")]
        public ICollection<string> AttributesValues { get; set; }
        [RequiredStringsIf(nameof(IsSimpleProduct), false,ErrorMessage = "لطفا نام تنوع را وارد نمایید")]
        public IEnumerable<string> ProductVariationsNames { get; set; }
        public IEnumerable<int?> ProductVariationsPrices { get; set; }
        public IEnumerable<int?> ProductVariationsQuantityInStocks { get; set; }

        [MaxFilesCount(6)]
        [MaxFilesSize(4194304)]
        [AllowedFilesExtensions(new string[] { ".png", ".jpg", ".jpeg" })]
        public IEnumerable<IFormFile> ProductImagesFiles { get; set; }
        public IEnumerable<int> ProductCategoriesId { get; set; }
        public IEnumerable<SelectListItem> ProductCategories { get; set; }

        public IEnumerable<string> InformationNames { get; set; }
        public IEnumerable<string> InformationValues { get; set; }

        //Properties For Editing Product

        public IEnumerable<Image> CurrentProductImages { get; set; }
        public IEnumerable<int> DeletedProductImagesIds { get; set; }
        public int ProductId { get; set; }

    }
}
