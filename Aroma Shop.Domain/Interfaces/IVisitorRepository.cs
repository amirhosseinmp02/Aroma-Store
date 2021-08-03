using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.VisitorModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IVisitorRepository : IGeneralRepository
    {
        IEnumerable<Visitor> GetVisitors();
        Visitor GetVisitorByIpAddress(string visitorIpAddress);
        int GetNumberOfVisits();
        void AddVisitor(Visitor visitor);   
        void UpdateVisitor(Visitor visitor);
    }
}
