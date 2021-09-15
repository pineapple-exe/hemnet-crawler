using Xunit;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Models;
using System.Linq;
using System;

namespace HemnetCrawler.Domain.Tests.Unit
{
    public class ListingQualitiesTest
    {
        [Fact]
        public void GetImageData_AddedImageData_CorrectImageData()
        {
            // Arrange
            FakeListingRepository listingRepository = new();
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            ListingQualities listingQualities = new(listingRepository, finalBidRepository, listingRatingRepository);

            listingRepository.AddImage(new Image() { Id = 1, Data = Array.Empty<byte>() });

            //Act
            byte[] imageData = listingQualities.GetImageData(1);

            //Assert
            Assert.Empty(imageData);
        }

        [Fact]
        public void GetImageData_NoAddedImageData_Exception()
        {
            // Arrange
            FakeListingRepository listingRepository = new();
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            ListingQualities listingQualities = new(listingRepository, finalBidRepository, listingRatingRepository);

            //Act-Assert
            Assert.Throws<InvalidOperationException>(() => listingQualities.GetImageData(1));
        }

        [Fact]
        public void GetEstimatedPrice_AddedFinalBids_AveragePrice()
        {
            // Arrange
            FakeListingRepository listingRepository = new();
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            ListingQualities listingQualities = new(listingRepository, finalBidRepository, listingRatingRepository);

            listingRepository.AddListing(new()
            {
                Id = 100,
                FinalBidId = 200
            });

            finalBidRepository.AddFinalBid(new()
            {
                Id = 200,
                Price = 3000
            });

            //Act
            double? averagePrice = listingQualities.GetEstimatedPrice(100);

            //Assert
            Assert.Equal(3000, averagePrice);
        }

        [Fact]
        public void GetEstimatedPrice_NoFinalBids_ReturnNull()
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
            double? averagePrice = listingQualities.GetEstimatedPrice(100);

            //Assert
            Assert.Null(averagePrice);
        }

        [Fact]
        public void AddListingRating_AddingListingRating_ListingRatingExists()
        {
            //Arrange
            FakeListingRepository listingRepository = new();
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            ListingQualities listingQualities = new(listingRepository, finalBidRepository, listingRatingRepository);

            //Act
            listingQualities.AddListingRating(100, 2, null);
            IQueryable<ListingRating> listingRatings = listingRatingRepository.GetAll();

            //Assert
            Assert.Single(listingRatings);
            Assert.Equal(2, listingRatings.First().KitchenRating);
            Assert.Null(listingRatings.First().BathroomRating);
        }

        [Fact]
        public void AddListingRating_NoListingRatings_Empty()
        {
            //Arrange
            FakeListingRatingRepository listingRatingRepository = new();

            //Act
            IQueryable<ListingRating> listingRatings = listingRatingRepository.GetAll();

            //Assert
            Assert.Empty(listingRatings);
        }

        [Fact]
        public void GetListingRating_AddedListingRatings_CorrectListingRating()
        {
            //Arrange
            FakeListingRepository listingRepository = new();
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            ListingQualities listingQualities = new(listingRepository, finalBidRepository, listingRatingRepository);

            //Act
            listingRatingRepository.listingRatings.Add(new ListingRating() { ListingId = 100, KitchenRating = null, BathroomRating = 1 });
            ListingRatingOutputModel model = listingQualities.GetListingRating(100);

            //Assert
            Assert.Null(model.KitchenRating);
            Assert.Equal(1, model.BathroomRating);
        }

        [Fact]
        public void GetListingRating_NoListingRatings_Empty()
        {
            //Arrange
            FakeListingRepository listingRepository = new();
            FakeFinalBidRepository finalBidRepository = new();
            FakeListingRatingRepository listingRatingRepository = new();

            ListingQualities listingQualities = new(listingRepository, finalBidRepository, listingRatingRepository);

            //Act
            listingRatingRepository.listingRatings.Add(new ListingRating() { ListingId = 100, KitchenRating = null, BathroomRating = 1 });
            ListingRatingOutputModel model = listingQualities.GetListingRating(100);

            //Assert
            Assert.Null(model.KitchenRating);
            Assert.Equal(1, model.BathroomRating);
        }


    }
}
