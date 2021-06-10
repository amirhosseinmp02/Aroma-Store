using System;
using System.Collections.Generic;
using System.Text;
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

        public Image GetImage(int imageId)
        {
            var image =
                _context.Images.Find(imageId);

            return image;
        }
        public void AddImage(Image image)
        {
            _context.Add(image);
        }
        public void DeleteImage(Image image)
        {
            _context.Remove(image);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
