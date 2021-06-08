using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;

namespace HemnetCrawler.Data.Repositories
{
    public class FinalBidRepository : IFinalBidRepository
    {
        private HemnetCrawlerDbContext _context;

        public FinalBidRepository(HemnetCrawlerDbContext context)
        {
            _context = context;
        }

        public void AddFinalBid(FinalBid finalBid)
        {
            _context.Add(finalBid);
            _context.SaveChanges();
        }

        public IQueryable<FinalBid> GetAll()
        {
            return _context.FinalBids;
        }
    }
}
