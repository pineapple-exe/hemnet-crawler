using System.Collections.Generic;
using System.Linq;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;

namespace HemnetCrawler.Domain.Tests.Unit
{
    internal class FakeFinalBidRepository : IFinalBidRepository
    {
        private readonly List<FinalBid> finalBids = new()
        {
            new FinalBid()
            {
                Id = 4487,
                City = "Sannegårdshamnen, Göteborgs kommun",
                PostalCode = 41760,
                HomeType = "Lägenhet",
            }
        };

        public void AddFinalBid(FinalBid finalBid)
        {
            finalBids.Add(finalBid);
        }

        public IQueryable<FinalBid> GetAll()
        {
            return finalBids.AsQueryable();
        }
    }
}
