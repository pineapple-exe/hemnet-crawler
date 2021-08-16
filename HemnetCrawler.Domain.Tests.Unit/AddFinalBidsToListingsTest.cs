using Xunit;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Interactors;
using System.Linq;
using System;
using System.Collections.Generic;

namespace HemnetCrawler.Domain.Tests.Unit
{
    public class AddFinalBidsToListingsTest
    {
        [Fact]
        public void AddFinalBidsToListings_MatchAlternatives_BestMatch()
        {
            // Vill: Bekräfta att Listings får FinalBidId tillhörande det FinalBid med flest likheter.

            // Arrange
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRepository listingRepository = new();

            FinalBid finalBidBestMatch = new()
            {
                SoldDate = DateTimeOffset.MaxValue,
                Id = 1,
                Street = "Månljusgatan 1C",
                HomeType = "Lägenhet",
                PostalCode = 41822,
                DemandedPrice = 200000,
                OwnershipType = "Bostadsrätt",
                Fee = 4000,
                Rooms = "3"
            };

            FinalBid finalBidNextBestMatch = new()
            {
                SoldDate = DateTimeOffset.MaxValue,
                Id = 2,
                Street = "Muslångatan 1C",
                HomeType = "Lägenhet",
                PostalCode = 41822,
                DemandedPrice = 200000,
                OwnershipType = "Bostadsrätt",
                Fee = 4000,
                Rooms = "3 rum"
            };

            FinalBid finalBidWorstMatch = new()
            {
                SoldDate = DateTimeOffset.MaxValue,
                Id = 3,
                Street = "Kokongvägen 3",
                PostalCode = 41410,
                OwnershipType = "Hyresrätt",
                Fee = 3000,
                Rooms = "1 rum"
            };

            finalBidRepository.AddFinalBid(finalBidBestMatch);
            finalBidRepository.AddFinalBid(finalBidNextBestMatch);
            finalBidRepository.AddFinalBid(finalBidWorstMatch);

            listingRepository.AddListing(new()
            {
                Published = DateTimeOffset.MinValue,
                Street = "Månljusgatan 2C",
                HomeType = "Lägenhet",
                PostalCode = 41822,
                Price = 200000,
                OwnershipType = "Bostadsrätt",
                Fee = 4000,
                Rooms = "3 rum"
            });

            FinalBidListingAssociater associater = new(listingRepository, finalBidRepository);

            // Act
            associater.AddFinalBidsToListings();

            // Assert
            IQueryable<Listing> allListings = listingRepository.GetAllListings();
            Assert.Single(allListings);
            Assert.Equal(1, allListings.First().FinalBidId);
        }

        [Fact]
        public void AddFinalBidsToListings_NoFinalBids_FinalBidIdNull()
        {
            // Arrange
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRepository listingRepository = new();

            listingRepository.AddListing(new()
            {
                Street = "Månljusgatan 2C",
                HomeType = "Lägenhet",
                PostalCode = 41822,
                Price = 200000,
                OwnershipType = "Bostadsrätt",
                Fee = 4000,
                Rooms = "3 rum"
            });

            FinalBidListingAssociater associater = new(listingRepository, finalBidRepository);

            // Act
            associater.AddFinalBidsToListings();

            // Assert
            Assert.Null(listingRepository.GetAllListings().First().FinalBidId);
        }
    }
}
