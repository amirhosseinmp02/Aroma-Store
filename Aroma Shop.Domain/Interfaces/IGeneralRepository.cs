using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IGeneralRepository
    {
        Task SaveAsync();
    }
}
