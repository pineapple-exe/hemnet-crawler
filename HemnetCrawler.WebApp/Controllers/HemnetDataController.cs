using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Models;

namespace HemnetCrawler.WebPage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HemnetDataController : ControllerBase
    {
        private readonly FetchFinalBids _fetchFinalBids;
        private readonly FetchListings _fetchListings;
        private readonly ListingQualities _listingQualities;

        public HemnetDataController(FetchFinalBids fetchFinalBids, FetchListings fetchListings, ListingQualities listingQualities)
        {
            _fetchFinalBids = fetchFinalBids;
            _fetchListings = fetchListings;
            _listingQualities = listingQualities;
        }

        [HttpGet("listings")]
        public List<ListingOutputModel> GetListings()
        {
            return _fetchListings.ListListings();
        }

        [HttpGet("listing")]
        public ListingOutputModel GetListing(int listingId)
        {
            return _fetchListings.ListListings().Find(l => l.Id == listingId);
        }

        [HttpGet("image")]
        public IActionResult GetImage(int imageId)
        {
           byte[] imageData = _listingQualities.GetImageData(imageId);

           return File(imageData, "image/jpeg");
        }

        [HttpGet("finalBid")]
        public FinalBidOutputModel GetFinalBid(int finalBidId)
        {
            return _fetchFinalBids.ListFinalBids().Find(fb => fb.Id == finalBidId);
        }

        [HttpGet("finalBids")]
        public List<FinalBidOutputModel> GetFinalBids()
        {
            return _fetchFinalBids.ListFinalBids();
        }

        [HttpGet("relevantFinalBids")]
        public IActionResult GetRelevantFinalBids(int listingId)
        {
            return Ok(new { finalBids = _fetchFinalBids.ListRelevantFinalBids(listingId) });
        }

        [HttpGet("estimatedPrice")]
        public IActionResult GetAveragePrice(int listingId)
        {
            return Ok(new { price = _listingQualities.GetAveragePrice(listingId) });
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
