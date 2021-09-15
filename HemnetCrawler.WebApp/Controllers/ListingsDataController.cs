using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Models;

namespace HemnetCrawler.WebPage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingsDataController : ControllerBase
    {
        private readonly FetchListings _fetchListings;
        private readonly ListingQualities _listingQualities;

        public ListingsDataController(FetchListings fetchListings, ListingQualities listingQualities)
        {
            _fetchListings = fetchListings;
            _listingQualities = listingQualities;
        }

        [HttpGet("listings")]
        public EntitiesPage<ListingOutputModel> GetListings(int page, int size)
        {
            return _fetchListings.ListListings(page, size);
        }

        [HttpGet("listing")]
        public ListingOutputModel GetListing(int listingId)
        {
            return _fetchListings.GetListing(listingId);
        }

        [HttpGet("image")]
        public IActionResult GetImage(int imageId)
        {
           byte[] imageData = _listingQualities.GetImageData(imageId);

           return File(imageData, "image/jpeg");
        }

        [HttpGet("estimatedPrice")]
        public IActionResult GetAveragePrice(int listingId)
        {
            return Ok(new { price = _listingQualities.GetEstimatedPrice(listingId) });
        }

        [HttpGet("listingRating")]
        public ListingRatingOutputModel GetListingRating(int listingId)
        {
            return _listingQualities.GetListingRating(listingId);
        }

        [HttpPost("rateListing")]
        public IActionResult AddListingRating(ListingRatingInputModel ratingModel)
        {
            _listingQualities.AddListingRating(ratingModel.ListingId, ratingModel.KitchenRating, ratingModel.BathroomRating);

            return Ok();
        }
    }
}
