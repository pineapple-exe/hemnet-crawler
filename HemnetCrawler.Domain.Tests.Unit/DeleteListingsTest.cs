using Xunit;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Tests.Unit.FakeRepositories;
using System.Linq;

namespace HemnetCrawler.Domain.Tests.Unit
{
    public class DeleteListingsTest
    {
        [Fact]
        public void DeleteListing_ListingWithImageAndRating_AllRelevantEntitiesGone()
        {
            //Arrange
            FakeListingRepository listingRepository = new();
            FakeListingRatingRepository ratingRepository = new();
            DeleteListings deleteListings = new(listingRepository, ratingRepository);

            listingRepository.AddListing(new Listing { Id = 1 });
            listingRepository.AddListing(new Listing { Id = 2 });
            listingRepository.AddImage(new Image { Id = 100, ListingId = 1 });
            ratingRepository.AddListingRating(new ListingRating { Id = 1000, ListingId = 1 });

            //Act
            deleteListings.DeleteListing(1);

            //Assert
            IQueryable<Listing> allListings = listingRepository.GetAllListings();
            Assert.Single(allListings);
            Assert.Equal(2, allListings.Single().Id);
        }
    }
}
