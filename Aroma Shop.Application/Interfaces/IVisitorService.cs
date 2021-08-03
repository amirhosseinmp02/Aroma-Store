using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.VisitorModels;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IVisitorService
    {
        IEnumerable<Visitor> GetVisitors();
        int GetNumberOfVisits();
        bool AddOrUpdateVisitor(string visitorIpAddress);
    }
}
