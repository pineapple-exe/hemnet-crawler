using System.Linq;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;

namespace HemnetCrawler.Data.Repositories
{
    public class FinalBidRepository : IFinalBidRepository
    {
        private readonly HemnetCrawlerDbContext _context;

        public FinalBidRepository(HemnetCrawlerDbContext context)
        {
            _context = context;
        }

        public void AddFinalBid(FinalBid finalBid)
        {
            _context.Add(finalBid);
            _context.SaveChanges();
        }

        public void UpdateFinalBid(FinalBid finalBid)
        {
            _context.Update(finalBid);
            _context.SaveChanges();
        }

        public IQueryable<FinalBid> GetAll()
        {
            return _context.FinalBids;
        }
    }
}
