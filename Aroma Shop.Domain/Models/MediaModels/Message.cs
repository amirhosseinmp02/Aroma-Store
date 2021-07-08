using System;
using System.ComponentModel.DataAnnotations;

namespace Aroma_Shop.Domain.Models.MediaModels
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public DateTime SubmitTime { get; set; }
        [MaxLength(250,ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام خود را وارد نمایید")]
        public string MessageSenderName { get; set; }
        [EmailAddress(ErrorMessage = "فیلد وارد شده ایمیل نمی باشد")]
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا آدرس ایمیل خود را وارد نمایید")]
        public string MessageSenderEmail { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا موضوع پیام خود را وارد نمایید")]
        public string MessageSubject { get; set; }
        [Required(ErrorMessage = "لطفا پیام خود را شرح دهید")]
        [MaxLength(2000, ErrorMessage = "حداکثر 2000 کارکتر مجاز می باشد")]
        public string MessageDescription { get; set; }
        public bool IsRead { get; set; }
        public bool IsReplied { get; set; }

        //Navigations Properties

        public MessageReply MessageReply { get; set; }
    }
}
