using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Application.ViewModels.Message
{
    public class MessageDetailViewModel
    {
        public Domain.Models.MediaModels.Message Message { get; set; }
        [Required(ErrorMessage = "لطفا پاسخ پیام را شرح دهید")]
        public string MessageReplyDescription { get; set; }

        public DateTime MessageSubmitTime { get; set; } 
    }
}
