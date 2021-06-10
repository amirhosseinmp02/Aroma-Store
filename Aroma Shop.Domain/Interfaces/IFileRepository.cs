using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.FileModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IFileRepository : IGeneralRepository
    {
        Image GetImage(int imageId);
        void AddImage(Image image);
        void DeleteImage(Image image);
    }
}
