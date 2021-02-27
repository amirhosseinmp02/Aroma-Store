using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        [MaxLength(150,ErrorMessage = "حداکثر 150 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام خود را وارد نمایید")]
        public string MessageSenderName { get; set; }
        [EmailAddress(ErrorMessage = "فیلد وارد شده ایمیل نمی باشد")]
        [Required(ErrorMessage = "لطفا آدرس ایمیل خود را وارد نمایید")]
        public string MessageSenderEmail { get; set; }
        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا موضوع پیام خود را وارد نمایید")]
        public string MessageSubject { get; set; }
        [Required(ErrorMessage = "لطفا پیام خود را شرح دهید")]
        [MaxLength(2000, ErrorMessage = "حداکثر 2000 کارکتر مجاز می باشد")]
        public string MessageDescription { get; set; }
    }
}
