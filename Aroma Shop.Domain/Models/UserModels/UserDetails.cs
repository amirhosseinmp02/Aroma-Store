using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomIdentityModels;

namespace Aroma_Shop.Domain.Models.UserModels
{
    public class UserDetails
    {
        [Key]
        public int UserDetailsId { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        public string FirstName { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        public string LastName { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        public string UserProvince { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        public string UserCity { get; set; }
        public string UserAddress { get; set; }
        [StringLength(10,MinimumLength = 10,ErrorMessage = "کد پستی شامل 10 رقم می باشد")]
        public string UserZipCode { get; set; }
    }
}
