using HemnetCrawler.Data;
using HemnetCrawler.Data.Repositories;
using HemnetCrawler.Domain.Entities;

namespace HemnetCrawler.MockTestData
{
    class Program
    {
        static void Main(string[] args)
        {
            HemnetCrawlerDbContext context = new();

            FinalBidRepository finalBidRepository = new(context);
            ListingRepository listingRepository = new(context);

            listingRepository.AddListing(new Listing()
            {
                Id = 1,
                FinalBidId = 100
            });

            finalBidRepository.AddFinalBid(new FinalBid()
            {
                Id = 100
            });
        }
    }
}
