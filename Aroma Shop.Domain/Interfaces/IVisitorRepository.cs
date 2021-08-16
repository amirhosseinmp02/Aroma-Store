using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Domain.Models.VisitorModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IVisitorRepository : IGeneralRepository
    {
        Task<IEnumerable<Visitor>> GetVisitorsAsync();  
        Task<Visitor> GetVisitorByIpAddressAsync(string visitorIpAddress);
        Task<int> GetNumberOfVisitsAsync();
        Task AddVisitorAsync(Visitor visitor);   
        void UpdateVisitor(Visitor visitor);
    }
}
