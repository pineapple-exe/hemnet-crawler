using Xunit;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Tests.Unit.FakeRepositories;
using System.Linq;
using System;

namespace HemnetCrawler.Domain.Tests.Unit
{
    public class FinalBidListingAssociaterTest
    {
        [Fact]
        public void AlgorithmAddFinalBidsToListings_MatchAlternatives_BestMatch()
        {
            // Arrange
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRepository listingRepository = new();

            FinalBid finalBidBestMatch = new()
            {
                SoldDate = DateTime.MaxValue,
                Id = 1,
                Street = "Månljusgatan 1C",
                HomeType = "Lägenhet",
                PostalCode = 41822,
                DemandedPrice = 200000,
                OwnershipType = "Bostadsrätt",
                Fee = 4000,
                Rooms = 3
            };

            FinalBid finalBidNextBestMatch = new()
            {
                SoldDate = DateTime.MaxValue,
                Id = 2,
                Street = "Muslångatan 1C",
                HomeType = "Lägenhet",
                PostalCode = 41822,
                DemandedPrice = 200000,
                OwnershipType = "Bostadsrätt",
                Fee = 4000,
                Rooms = 3
            };

            FinalBid finalBidWorstMatch = new()
            {
                SoldDate = DateTime.MaxValue,
                Id = 3,
                Street = "Kokongvägen 3",
                PostalCode = 41410,
                OwnershipType = "Hyresrätt",
                Fee = 3000,
                Rooms = 1
            };

            finalBidRepository.AddFinalBid(finalBidBestMatch);
            finalBidRepository.AddFinalBid(finalBidNextBestMatch);
            finalBidRepository.AddFinalBid(finalBidWorstMatch);

            listingRepository.AddListing(new()
            {
                Published = DateTime.MinValue,
                Street = "Månljusgatan 2C",
                HomeType = "Lägenhet",
                PostalCode = 41822,
                Price = 200000,
                OwnershipType = "Bostadsrätt",
                Fee = 4000,
                Rooms = 3
            });

            FinalBidListingAssociater associater = new(listingRepository, finalBidRepository);

            // Act
            associater.AlgorithmAddFinalBidsToListings();

            // Assert
            IQueryable<Listing> allListings = listingRepository.GetAllListings();
            Assert.Single(allListings);
            Assert.Equal(1, allListings.First().FinalBidId);
        }

        [Fact]
        public void AlgorithmAddFinalBidsToListings_NoFinalBids_FinalBidIdNull()
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
                Rooms = 3
            });

            FinalBidListingAssociater associater = new(listingRepository, finalBidRepository);

            // Act
            associater.AlgorithmAddFinalBidsToListings();

            // Assert
            Assert.Null(listingRepository.GetAllListings().First().FinalBidId);
        }
    }
}
