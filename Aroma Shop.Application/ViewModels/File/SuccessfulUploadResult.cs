using System;
using System.Collections.Generic;
using System.Text;

namespace Aroma_Shop.Application.ViewModels.File
{
    public class SuccessfulUploadResult
    {
        public int Uploaded { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
    }
}
