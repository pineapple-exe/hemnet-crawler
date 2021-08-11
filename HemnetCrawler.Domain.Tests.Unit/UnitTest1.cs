using Xunit;
using HemnetCrawler.Domain.Entities;
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

            finalBidRepository.FinalBids.Add(new FinalBid()
            {
                Id = 4487,
                City = "Sanneg�rdshamnen, G�teborgs kommun",
                PostalCode = 41760,
                HomeType = "L�genhet",
            });

            listingRepository.Listings.Add(new Listing()
            {
                Id = 729,
                PostalCode = 41760,
                FinalBidID = 4487,
                HomeType = "L�genhet"
            });

            listingRepository.Listings.Add(new Listing()
            {
                Id = 2649,
                PostalCode = 41760,
                HomeType = "L�genhet"
            });

            FetchFinalBids fetchFinalBids = new(finalBidRepository, listingRepository, listingRatingRepository);

            // Act
            List<FinalBidEstimationOutputModel> output = fetchFinalBids.ListRelevantFinalBids(2649);

            // Assert
            Assert.Single(output);
        }
    }
}