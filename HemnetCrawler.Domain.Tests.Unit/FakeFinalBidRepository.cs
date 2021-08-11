using System.Collections.Generic;
using System.Linq;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;

namespace HemnetCrawler.Domain.Tests.Unit
{
    internal class FakeFinalBidRepository : IFinalBidRepository
    {
        public List<FinalBid> FinalBids = new();

        public void AddFinalBid(FinalBid finalBid)
        {
            FinalBids.Add(finalBid);
        }

        public IQueryable<FinalBid> GetAll()
        {
            return FinalBids.AsQueryable();
        }
    }
}
