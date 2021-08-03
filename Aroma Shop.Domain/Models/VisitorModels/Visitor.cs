using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.VisitorModels
{
    public class Visitor
    {
        [Key]
        public int VisitorId { get; set; }
        [MaxLength(50)]
        [Required]
        public string VisitorIpAddress { get; set; }
        [Required]
        public int CountOfVisit { get; set; }
        [Required]
        public DateTime LastVisitTime { get; set; } 
    }
}
