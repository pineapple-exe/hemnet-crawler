using Xunit;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Interactors;
using System.Linq;

namespace HemnetCrawler.Domain.Tests.Unit
{
    public class DeleteFinalBidsTest
    {
        [Fact]
        public void DeleteFinalBid_FinalBidWithListing_FinalBidAndFinalBidIdGone()
        {
            //Arrange
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRepository listingRepository = new();
            DeleteFinalBids deleteFinalBids = new(finalBidRepository, listingRepository);

            finalBidRepository.AddFinalBid(new FinalBid { Id = 1 });
            listingRepository.AddListing(new Listing { Id = 100, FinalBidId = 1 });

            //Act
            deleteFinalBids.DeleteFinalBid(1);

            //Assert
            Assert.Empty(finalBidRepository.GetAll());

            Listing listing = listingRepository.GetAllListings().Single();
            Assert.Null(listing.FinalBidId);
        }
    }
}
