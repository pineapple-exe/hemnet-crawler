﻿using Xunit;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Models;
using System.Collections.Generic;

namespace HemnetCrawler.Domain.Tests.Unit
{
    public class FetchListingsTest
    {
        [Fact]
        public void GetListing_ExistingListing_CorrectListing()
        {
            //Arrange
            FakeListingRepository repository = new();
            FetchListings fetchListings = new(repository);

            repository.Listings.AddRange(new List<Listing>()
            {
                new Listing() { Id = 1 },
                new Listing() { Id = 2 },
                new Listing() { Id = 3 }
            });

            //Act
            ListingOutputModel listingModel = fetchListings.GetListing(2);

            //Assert
            Assert.Equal(2, listingModel.Id);
        }

        [Fact]
        public void GetListing_NonExistingListing_Exception()
        {
            //Arrange
            FakeListingRepository repository = new();
            FetchListings fetchListings = new(repository);

            repository.Listings.AddRange(new List<Listing>()
            {
                new Listing() { Id = 1 },
                new Listing() { Id = 2 },
                new Listing() { Id = 3 }
            });

            //Act & Assert
            Assert.Throws<NotFoundException>(() => fetchListings.GetListing(4));
        }

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
            EntitiesPage<ListingOutputModel> models = fetchListings.ListListings(1, 2);

            //Assert
            Assert.Equal(2, models.Subset.Count);

            Assert.Equal(3, models.Subset[0].Id);
            Assert.Equal(4, models.Subset[1].Id);

            Assert.Equal(6, models.Total);
        }

        [Fact]
        public void ListListings_NoListings_Empty()
        {
            //Arrange
            FakeListingRepository repository = new();
            FetchListings fetchListings = new(repository);

            //Act
            EntitiesPage<ListingOutputModel> models = fetchListings.ListListings(1, 2);

            //Assert
            Assert.Empty(models.Subset);
        }
    }
}
