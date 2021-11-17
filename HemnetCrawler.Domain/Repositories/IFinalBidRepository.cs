using System.Linq;
using HemnetCrawler.Domain.Entities;

namespace HemnetCrawler.Domain.Repositories
{
    public interface IFinalBidRepository
    {
        void AddFinalBid(FinalBid finalBid);
        void UpdateFinalBid(FinalBid finalBid);
        IQueryable<FinalBid> GetAll();
        void DeleteFinalBid(FinalBid finalBid);
    }
}
