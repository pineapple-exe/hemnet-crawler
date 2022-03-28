using Xunit;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Models;
using HemnetCrawler.Domain.Tests.Unit.FakeRepositories;
using System.Collections.Generic;
using System.Linq;

namespace HemnetCrawler.Domain.Tests.Unit
{
    public class FetchFinalBidsTest
    {
        [Fact]
        public void ListRelevantFinalBids_AddedListingsAndFinalBid_OneRelevance()
        {
            //Arrange
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRepository listingRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            finalBidRepository.FinalBids.Add(new FinalBid()
            {
                Id = 4487,
                City = "Sannegårdshamnen, Göteborgs kommun",
                PostalCode = 41760,
                HomeType = "Lägenhet",
            });

            listingRepository.Listings.Add(new Listing()
            {
                Id = 729,
                PostalCode = 41760,
                FinalBidId = 4487,
                HomeType = "Lägenhet"
            });

            listingRepository.Listings.Add(new Listing()
            {
                Id = 2649,
                PostalCode = 41760,
                HomeType = "Lägenhet"
            });

            FetchFinalBids fetchFinalBids = new(finalBidRepository, listingRepository, listingRatingRepository);

            //Act
            List<FinalBidEstimationOutputModel> output = fetchFinalBids.ListRelevantFinalBids(2649);

            //Assert
            Assert.Single(output);
        }

        [Fact]
        public void ListRelevantFinalBids_ListingButNoFinalBids_NoRelevance()
        {
            //Arrange
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRepository listingRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            FetchFinalBids fetchFinalBids = new(finalBidRepository, listingRepository, listingRatingRepository);

            listingRepository.Listings.Add(new Listing()
            {
                Id = 729
            });

            //Act
            List<FinalBidEstimationOutputModel> output = fetchFinalBids.ListRelevantFinalBids(729);

            //Assert
            Assert.Empty(output);
        }

        [Fact]
        public void ListFinalBids_AddedFinalBidWithoutListing_ListingIdIsNull()
        {
            //Arrange
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRepository listingRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            FetchFinalBids fetchFinalBids = new(finalBidRepository, listingRepository, listingRatingRepository);

            FinalBidsFilterInputModel filter = new();

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
            FinalBidOutputModel output = fetchFinalBids.ListFinalBids(0, 20, filter, SortDirection.Ascending).Items.First();

            //Assert
            Assert.Null(output.ListingId);
        }

        [Fact]
        public void ListFinalBids_MoreFinalBidsThanPageSize_CorrectSubsetAndTotal()
        {
            //Arrange
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRepository listingRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            FetchFinalBids fetchFinalBids = new(finalBidRepository, listingRepository, listingRatingRepository);

            FinalBidsFilterInputModel filter = new();

            finalBidRepository.FinalBids.AddRange(new List<FinalBid>() 
            {
                new FinalBid() { Id = 1 },
                new FinalBid() { Id = 2 },
                new FinalBid() { Id = 3 },
                new FinalBid() { Id = 4 },
                new FinalBid() { Id = 5 },
                new FinalBid() { Id = 6 }
            });

            //Act
            ItemsPage<FinalBidOutputModel> output = fetchFinalBids.ListFinalBids(1, 2, filter, SortDirection.Ascending);

            //Assert
            Assert.Equal(2, output.Items.Count);

            Assert.Equal(3, output.Items[0].Id);
            Assert.Equal(4, output.Items[1].Id);

            Assert.Equal(6, output.Total);
        }

        [Fact]
        public void GetFinalBid_ExistingFinalBid_CorrectFinalBid()
        {
            //Arrange
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRepository listingRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            FetchFinalBids fetchFinalBids = new(finalBidRepository, listingRepository, listingRatingRepository);

            finalBidRepository.FinalBids.AddRange(new List<FinalBid>()
            {
                new FinalBid() { Id = 1 },
                new FinalBid() { Id = 2 },
                new FinalBid() { Id = 3 }
            });

            //Act
            FinalBidOutputModel outputModel = fetchFinalBids.GetFinalBid(3);

            //Assert
            Assert.Equal(3, outputModel.Id);
        }

        [Fact]
        public void GetFinalBid_NonExistingId_Exception()
        {
            //Arrange
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRepository listingRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            FetchFinalBids fetchFinalBids = new(finalBidRepository, listingRepository, listingRatingRepository);

            finalBidRepository.FinalBids.AddRange(new List<FinalBid>()
            {
                new FinalBid() { Id = 1 },
                new FinalBid() { Id = 2 },
                new FinalBid() { Id = 3 }
            });

            //Act-Assert
            Assert.Throws<NotFoundException>(() => fetchFinalBids.GetFinalBid(4));
        }
    }
}