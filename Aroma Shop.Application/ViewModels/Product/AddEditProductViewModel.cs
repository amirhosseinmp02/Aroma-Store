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
            MixedProductAttributesNames = new List<string>();
            MixedProductAttributesPrices = new List<double?>();
            MixedProductAttributesQuantityInStocks = new List<int?>();
            InformationNames = new List<string>();
            InformationValues = new List<string>();
        }

        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام محصول را وارد نمایید")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "لطفا قیمت محصول را وارد نمایید")]
        [Range(0, 999999999, ErrorMessage = "حداقل مقدار 0 می باشد")]
        public double ProductPrice { get; set; }
        [Required(ErrorMessage = "لطفا تعداد محصول را وارد نمایید")]
        [Range(0, 999999999, ErrorMessage = "حداقل مقدار 0 می باشد")]
        public int ProductQuantityInStock { get; set; }
        [MaxLength(10000, ErrorMessage = "حداکثر 10000 کارکتر مجاز می باشد")]
        public string ProductDescription { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        public string ProductShortDescription { get; set; }

        [Required(ErrorMessage = "لطفا نوع محصول را انتخاب نمایید")]
        public bool IsSimpleProduct { get; set; } = true;
        [RequiredStrings]
        public IEnumerable<string> AttributesNames { get; set; }
        [RequiredStrings]
        public IEnumerable<string> AttributesValues { get; set; }
        [RequiredStrings]
        public IEnumerable<string> MixedProductAttributesNames { get; set; }
        public IEnumerable<double?> MixedProductAttributesPrices { get; set; }
        public IEnumerable<int?> MixedProductAttributesQuantityInStocks { get; set; }

        [MaxFilesCount(6)]
        [MaxFileSize(4194304)]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".jpeg" })]
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
