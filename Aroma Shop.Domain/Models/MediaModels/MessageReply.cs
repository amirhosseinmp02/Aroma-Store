using System;
using System.ComponentModel.DataAnnotations;

namespace Aroma_Shop.Domain.Models.MediaModels
{
    public class MessageReply
    {
        public int MessageReplyId { get; set; }
        public DateTime MessageReplySubmitTime { get; set; }
        [Required(ErrorMessage = "لطفا پاسخ پیام را شرح دهید")]
        public string MessageReplyDescription { get; set; }
    }
}
