using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HemnetCrawler.Domain;
using HemnetCrawler.Domain.Models;
using HemnetCrawler.WebApp.Models;

namespace HemnetCrawler.WebPage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HemnetDataController : ControllerBase
    {
        private readonly HemnetCrawlerInteractor _hemnetCrawlerInteractor;

        public HemnetDataController(HemnetCrawlerInteractor hemnetCrawlerInteractor)
        {
            _hemnetCrawlerInteractor = hemnetCrawlerInteractor;
        }

        [HttpGet("listings")]
        public List<ListingOutputModel> GetListings()
        {
            return _hemnetCrawlerInteractor.ListListings();
        }

        [HttpGet("listing")]
        public ListingOutputModel GetListing(int listingId)
        {
            return _hemnetCrawlerInteractor.ListListings().Find(l => l.Id == listingId);
        }

        [HttpGet("image")]
        public IActionResult GetImage(int imageId)
        {
           byte[] imageData = _hemnetCrawlerInteractor.GetImageData(imageId);

           return File(imageData, "image/jpeg");
        }

        [HttpGet("finalBids")]
        public List<FinalBidOutputModel> GetFinalBids()
        {
            return _hemnetCrawlerInteractor.ListFinalBids();
        }

        [HttpGet("estimatedPrice")]
        public IActionResult GetAveragePrice(int listingId)
        {
            return Ok(new { price = _hemnetCrawlerInteractor.GetAveragePrice(listingId) });
        }

        [HttpGet("listingRating")]
        public ListingRatingOutputModel GetListingRating(int listingId)
        {
            return _hemnetCrawlerInteractor.GetListingRating(listingId);
        }

        [HttpPost("rateListing")]
        public IActionResult AddListingRating(ListingRatingInputModel ratingModel)
        {
            _hemnetCrawlerInteractor.AddListingRating(ratingModel.ListingId, ratingModel.KitchenRating, ratingModel.BathroomRating);

            return Ok();
        }
    }
}
