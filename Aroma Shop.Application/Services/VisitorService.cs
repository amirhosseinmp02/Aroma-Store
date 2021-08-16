using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.VisitorModels;

namespace Aroma_Shop.Application.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly IVisitorRepository _visitorRepository;

        public VisitorService(IVisitorRepository visitorRepository)
        {
            _visitorRepository = visitorRepository;
        }

        public async Task<IEnumerable<Visitor>> GetVisitorsAsync()
        {
            var visitors =
                await _visitorRepository
                    .GetVisitorsAsync();

            return visitors;
        }
        public async Task<int> GetNumberOfVisitsAsync()
        {
            var numberOfVisits =
                await _visitorRepository
                    .GetNumberOfVisitsAsync();

            return numberOfVisits;
        }
        public async Task<bool> AddOrUpdateVisitorAsync(string visitorIpAddress)
        {
            try
            {
                var visitor =
                    await _visitorRepository
                        .GetVisitorByIpAddressAsync(visitorIpAddress);

                if (visitor != null)
                {
                    ++visitor.CountOfVisit;

                    visitor.LastVisitTime =
                        DateTime.Now;

                    _visitorRepository
                        .UpdateVisitor(visitor);
                }
                else
                {
                    visitor = new Visitor()
                    {
                        VisitorIpAddress = visitorIpAddress,
                        CountOfVisit = 1,
                        LastVisitTime = DateTime.Now
                    };

                    await _visitorRepository
                        .AddVisitorAsync(visitor);
                }

                await _visitorRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
    }
}
