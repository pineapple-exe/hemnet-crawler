using Xunit;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Models;
using System.Collections.Generic;

namespace HemnetCrawler.Domain.Tests.Unit
{
    public class Tests
    {
        [Fact]
        public void TestListRelevantFinalBids()
        {
            // Arrange
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRepository listingRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            FetchFinalBids fetchFinalBids = new(finalBidRepository, listingRepository, listingRatingRepository);

            // Act
            List<FinalBidOutputModel> output = fetchFinalBids.ListRelevantFinalBids(2649);

            // Assert
            Assert.NotEmpty(output);
        }
    }
}