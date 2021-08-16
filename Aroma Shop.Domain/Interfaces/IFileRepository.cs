using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Domain.Models.FileModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IFileRepository : IGeneralRepository
    {
        Task<Image> GetImageAsync(int imageId);
        Task AddImageAsync(Image image);
        void DeleteImageAsync(Image image);
    }
}
