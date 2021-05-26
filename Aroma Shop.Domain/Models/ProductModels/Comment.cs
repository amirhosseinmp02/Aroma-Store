using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomIdentityModels;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class Comment
    {
        public Comment()
        {
            Replies = new List<Comment>();
        }

        [Key]
        public int CommentId { get; set; }

        public DateTime SubmitTime { get; set; }
        [Required(ErrorMessage = "لطفا نظر خود را وارد کنید")]
        [MaxLength(10000, ErrorMessage = "حداکثر 10000 کارکتر مجاز می باشد")]
        public string CommentDescription { get; set; }
        public bool IsRead { get; set; }
        public bool IsConfirmed { get; set; }

        //Navigations Proterties

        public Comment ParentComment { get; set; }  
        public ICollection<Comment> Replies { get; set; }
        public Product Product { get; set; }
        public CustomIdentityUser User { get; set; }
    }
}
