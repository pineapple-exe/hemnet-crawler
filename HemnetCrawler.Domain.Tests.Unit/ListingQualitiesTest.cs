using Xunit;
using HemnetCrawler.Domain.Interactors;

namespace HemnetCrawler.Domain.Tests.Unit
{
    public class ListingQualitiesTest
    {
        [Fact]
        public void GetAveragePrice_NoFinalBids_ReturnNull()
        {
            // Arrange

            FakeListingRepository listingRepository = new();
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            ListingQualities listingQualities = new(listingRepository, finalBidRepository, listingRatingRepository);

            listingRepository.AddListing(new()
            {
                Id = 100
            });

            //Act

            double? averagePrice = listingQualities.GetAveragePrice(100);

            //Assert

            Assert.Null(averagePrice);
        }
    }
}
