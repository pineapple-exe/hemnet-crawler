using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HemnetCrawler.Domain.Entities;

namespace HemnetCrawler.Domain.Repositories
{
    public interface IListingRepository
    {
        void AddListing(Listing listing);
        void AddImage(Image image);
        bool IsFinalBidAMatch(Listing listing, FinalBid finalBid);
        void AddFinalBidToListing();
        void DisposeContext();
    }
}
