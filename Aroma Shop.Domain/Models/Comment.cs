using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models
{
    public class Comment
    {
        public Comment()
        {
            Replies = new List<Comment>();
        }

        [Key]
        public int CommentId { get; set; }
        [MaxLength(100, ErrorMessage = "حداکثر 150 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام خود را وارد نمایید")]
        public string CommentName { get; set; }
        [Required]
        [EmailAddress]
        public string CommentEmail { get; set; }
        [Required(ErrorMessage = "لطفا نظر خود را تکمیل کنید")]
        [MaxLength(10000, ErrorMessage = "حداکثر 10000 کارکتر مجاز می باشد")]
        public string CommentMessage { get; set; }

        //Navigations Proterties

        public ICollection<Comment> Replies { get; set; }
        public Product Product { get; set; }
    }
}
