using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomIdentityModels;

namespace Aroma_Shop.Domain.Models.UserModels
{
    public class UserDetail
    {
        [Key]
        public int UserDetailId { get; set; }
        [MaxLength(150, ErrorMessage = "حداکثر 150 کارکتر مجاز می باشد")]
        public string FirstName { get; set; }
        [MaxLength(150, ErrorMessage = "حداکثر 150 کارکتر مجاز می باشد")]
        public string LastName { get; set; }
        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        public string UserProvince { get; set; }
        [MaxLength(150, ErrorMessage = "حداکثر 150 کارکتر مجاز می باشد")]
        public string UserCity { get; set; }
        [MaxLength(2000, ErrorMessage = "حداکثر 2000 کارکتر مجاز می باشد")]
        public string UserAddress { get; set; }
        [StringLength(10,MinimumLength = 10,ErrorMessage = "کد پستی شامل 10 رقم می باشد")]
        public string UserZipCode { get; set; }
    }
}
