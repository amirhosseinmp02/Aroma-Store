using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.VisitorModels;

namespace Aroma_Shop.Data.Repositories
{
    public class VisitorRepository : IVisitorRepository
    {
        private readonly AppDbContext _context;

        public VisitorRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Visitor> GetVisitors()
        {
            var visitors =
                _context
                    .Visitors;

            return visitors;
        }
        public Visitor GetVisitorByIpAddress(string visitorIpAddress)
        {
            var visitor =
                _context
                    .Visitors
                    .SingleOrDefault(p => p.VisitorIpAddress == visitorIpAddress);

            return visitor;
        }
        public int GetNumberOfVisits()
        {
            var numberOfVisits =
                _context
                    .Visitors
                    .Sum(p => p.CountOfVisit);

            return numberOfVisits;
        }
        public void AddVisitor(Visitor visitor)
        {
            _context
                .Add(visitor);
        }
        public void UpdateVisitor(Visitor visitor)
        {
            _context
                .Update(visitor);
        }

        public void Save()
        {
            _context
                .SaveChanges();
        }
    }
}
