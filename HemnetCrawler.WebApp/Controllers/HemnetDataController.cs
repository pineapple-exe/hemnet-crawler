using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HemnetCrawler.Domain;

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

        [HttpGet("finalBids")]
        public List<FinalBidOutputModel> GetFinalBids()
        {
            return _hemnetCrawlerInteractor.ListFinalBids();
        }
    }
}
