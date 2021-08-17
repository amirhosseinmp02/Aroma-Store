using System;
using System.Collections.Generic;
using System.Text;

namespace Aroma_Shop.Application.ViewModels.IdPayModels
{
    public class IdPayRequestModel
    {
        public int order_id { get; set; }
        public int amount { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public string desc { get; set; }
        public string callback { get; set; }
    }
}
