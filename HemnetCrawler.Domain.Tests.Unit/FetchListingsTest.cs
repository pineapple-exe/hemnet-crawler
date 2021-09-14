using Xunit;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Models;
using System.Collections.Generic;

namespace HemnetCrawler.Domain.Tests.Unit
{
    public class FetchListingsTest
    {
        [Fact]
        public void ListListings_MoreListingsThanPageSize_CorrectSubsetAndTotal()
        {
            //Arrange
            FakeListingRepository repository = new();
            FetchListings fetchListings = new(repository);

            repository.Listings.AddRange(new List<Listing>() 
            { 
                new Listing() { Id = 1 },
                new Listing() { Id = 2 },
                new Listing() { Id = 3 },
                new Listing() { Id = 4 },
                new Listing() { Id = 5 },
                new Listing() { Id = 6 } 
            });

            //Act
            EntitiesPage<ListingOutputModel> listingsOutputModel = fetchListings.ListListings(1, 2);

            //Assert
            Assert.Equal(2, listingsOutputModel.ListingsSubset.Count);

            Assert.Equal(3, listingsOutputModel.ListingsSubset[0].Id);
            Assert.Equal(4, listingsOutputModel.ListingsSubset[1].Id);

            Assert.Equal(6, listingsOutputModel.Total);
        }
    }
}
