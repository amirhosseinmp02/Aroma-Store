using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Domain.Models.VisitorModels;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IVisitorService
    {
        Task<IEnumerable<Visitor>> GetVisitorsAsync();
        Task<int> GetNumberOfVisitsAsync();
        Task<bool> AddOrUpdateVisitorAsync(string visitorIpAddress);
    }
}
