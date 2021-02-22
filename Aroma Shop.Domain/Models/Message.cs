using System;
using System.Collections.Generic;
using System.Text;

namespace Aroma_Shop.Domain.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string MessageSenderName { get; set; }
        public string MessageSenderEmail { get; set; }
        public string MessageSubject { get; set; }
        public string MessageDescription { get; set; }
    }
}
