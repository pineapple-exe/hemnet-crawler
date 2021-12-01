using Microsoft.AspNetCore.Mvc;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Models;
using HemnetCrawler.Domain;

namespace HemnetCrawler.WebPage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingsDataController : ControllerBase
    {
        private readonly FetchListings _fetchListings;
        private readonly ListingQualities _listingQualities;
        private readonly DeleteListings _deleteListings;

        public ListingsDataController(FetchListings fetchListings, ListingQualities listingQualities, DeleteListings deleteListings)
        {
            _fetchListings = fetchListings;
            _listingQualities = listingQualities;
            _deleteListings = deleteListings;
        }

        [HttpGet("listings")]
        public ItemsPage<ListingOutputModel> GetListings(int pageIndex, int size, SortDirection sortDirection, string by)
        {
            return _fetchListings.ListListings(pageIndex, size, sortDirection, by.ToLower());
        }

        [HttpGet("listing")]
        public ListingOutputModel GetListing(int listingId)
        {
            return _fetchListings.GetListing(listingId);
        }

        [HttpGet("image")]
        public IActionResult GetImage(int imageId)
        {
           ImageOutputModel image = _listingQualities.GetImage(imageId);

           return File(image.Data, string.IsNullOrEmpty(image.ContentType) ? "image/jpeg" : image.ContentType);
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

        [HttpDelete("deleteListing")]
        public IActionResult DeleteListing(int listingId)
        {
            _deleteListings.DeleteListing(listingId);

            return Ok();
        }
    }
}
