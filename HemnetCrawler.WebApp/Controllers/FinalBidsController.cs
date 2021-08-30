using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Models;

namespace HemnetCrawler.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinalBidsController
    {
        private readonly FetchFinalBids _fetchFinalBids;

        public FinalBidsController(FetchFinalBids fetchFinalBids)
        {
            _fetchFinalBids = fetchFinalBids;
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
    }
}
