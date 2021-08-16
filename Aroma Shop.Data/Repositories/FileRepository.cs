using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.FileModels;

namespace Aroma_Shop.Data.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;

        public FileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Image> GetImageAsync(int imageId)
        {
            var image =
                await _context
                    .Images
                    .FindAsync(imageId);

            return image;
        }
        public async Task AddImageAsync(Image image)
        {
            await _context
                .AddAsync(image);
        }
        public void DeleteImageAsync(Image image)
        {
            _context.Remove(image);
        }
        public async Task SaveAsync()
        {
            await _context
                .SaveChangesAsync();
        }
    }
}
