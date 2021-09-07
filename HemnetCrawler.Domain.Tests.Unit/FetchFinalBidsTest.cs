using Xunit;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace HemnetCrawler.Domain.Tests.Unit
{
    public class FetchFinalBidsTest
    {
        [Fact]
        public void ListRelevantFinalBids_AddedListingsAndFinalBid_OneRelevance()
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
                FinalBidId = 4487,
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

        [Fact]
        public void ListFinalBids_AddedFinalBidWithoutListing_ListingIdIsNull()
        {
            //Arrange
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRepository listingRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            FetchFinalBids fetchFinalBids = new(finalBidRepository, listingRepository, listingRatingRepository);

            finalBidRepository.FinalBids.Add(new()
            {
                Id = 1,
            });

            listingRepository.Listings.Add(new()
            {
                Id = 2,
                FinalBidId = 3,
            });

            //Act
            FinalBidOutputModel output = fetchFinalBids.ListFinalBids().First();

            //Assert
            Assert.Null(output.ListingId);
        }
    }
}