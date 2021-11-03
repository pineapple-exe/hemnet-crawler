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

        public void DeleteFinalBid(FinalBid finalBid)
        {
            FinalBids.Remove(finalBid);
        }

        public IQueryable<FinalBid> GetAll()
        {
            return FinalBids.AsQueryable();
        }

        public void UpdateFinalBid(FinalBid finalBid)
        {
            FinalBids.RemoveAll(fb => fb.Id == finalBid.Id);
            FinalBids.Add((FinalBid)finalBid.Clone());
        }
    }
}
