using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Mvc.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        #region UploadEditorFile

        [HttpPost("/UploadEditorFile")]
        [IgnoreAntiforgeryToken]
        public IActionResult UploadEditorFile(IFormFile upload)
        {
            var isLocal = 
                Request.IsLocal();

            if (!isLocal)
                return NotFound();

            var result =
                _fileService.UploadEditorFile(upload);

            return result;
        }

        #endregion
    }
}
