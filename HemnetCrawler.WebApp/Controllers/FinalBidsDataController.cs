using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Models;

namespace HemnetCrawler.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinalBidsDataController : ControllerBase
    {
        private readonly FetchFinalBids _fetchFinalBids;

        public FinalBidsDataController(FetchFinalBids fetchFinalBids)
        {
            _fetchFinalBids = fetchFinalBids;
        }

        [HttpGet("finalBid")]
        public FinalBidOutputModel GetFinalBid(int finalBidId)
        {
            return _fetchFinalBids.GetFinalBid(finalBidId);
        }

        [HttpGet("finalBids")]
        public FinalBidsOutputModel GetFinalBids(int page, int size)
        {
            return _fetchFinalBids.ListFinalBids(page, size);
        }

        [HttpGet("relevantFinalBids")]
        public IActionResult GetRelevantFinalBids(int listingId)
        {
            return Ok(new { finalBids = _fetchFinalBids.ListRelevantFinalBids(listingId) });
        }
    }
}
