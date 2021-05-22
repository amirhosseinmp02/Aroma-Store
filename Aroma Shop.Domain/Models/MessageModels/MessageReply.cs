using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.MessageModels
{
    public class MessageReply
    {
        public int MessageReplyId { get; set; }
        public DateTime MessageReplySubmitTime { get; set; }
        [Required(ErrorMessage = "لطفا پاسخ پیام را شرح دهید")]
        public string MessageReplyDescription { get; set; }
    }
}
