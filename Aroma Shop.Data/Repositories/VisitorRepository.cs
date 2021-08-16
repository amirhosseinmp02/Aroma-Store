using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.VisitorModels;
using Microsoft.EntityFrameworkCore;

namespace Aroma_Shop.Data.Repositories
{
    public class VisitorRepository : IVisitorRepository
    {
        private readonly AppDbContext _context;

        public VisitorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Visitor>> GetVisitorsAsync()
        {
            var visitors =
                await _context
                    .Visitors
                    .ToListAsync();

            return visitors;
        }
        public async Task<Visitor> GetVisitorByIpAddressAsync(string visitorIpAddress)
        {
            var visitor =
                await _context
                    .Visitors
                    .SingleOrDefaultAsync(p => p.VisitorIpAddress == visitorIpAddress);

            return visitor;
        }
        public async Task<int> GetNumberOfVisitsAsync()
        {
            var numberOfVisits =
                await _context
                    .Visitors
                    .SumAsync(p => p.CountOfVisit);

            return numberOfVisits;
        }
        public async Task AddVisitorAsync(Visitor visitor)
        {
            await _context
                .AddAsync(visitor);
        }
        public void UpdateVisitor(Visitor visitor)
        { 
            _context
                .Update(visitor);
        }

        public async Task SaveAsync()
        {
            await _context
                .SaveChangesAsync();
        }
    }
}
