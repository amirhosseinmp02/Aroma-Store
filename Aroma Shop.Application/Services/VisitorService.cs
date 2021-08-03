using System;
using System.Collections.Generic;
using System.Text;
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

        public IEnumerable<Visitor> GetVisitors()
        {
            var visitors =
                _visitorRepository
                    .GetVisitors();

            return visitors;
        }
        public int GetNumberOfVisits()
        {
            var numberOfVisits =
                _visitorRepository
                    .GetNumberOfVisits();

            return numberOfVisits;
        }
        public bool AddOrUpdateVisitor(string visitorIpAddress)
        {
            try
            {
                var visitor =
                    _visitorRepository
                        .GetVisitorByIpAddress(visitorIpAddress);

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

                    _visitorRepository
                        .AddVisitor(visitor);
                }

                _visitorRepository
                    .Save();

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
