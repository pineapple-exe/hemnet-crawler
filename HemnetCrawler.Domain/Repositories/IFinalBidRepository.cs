using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HemnetCrawler.Domain.Entities;

namespace HemnetCrawler.Domain.Repositories
{
    public interface IFinalBidRepository
    {
        void AddFinalBid(FinalBid finalBid);
        IQueryable<FinalBid> GetAll();
    }
}
