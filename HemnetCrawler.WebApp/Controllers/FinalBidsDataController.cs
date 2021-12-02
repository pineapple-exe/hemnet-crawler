using Microsoft.AspNetCore.Mvc;
using HemnetCrawler.Domain.Interactors;
using HemnetCrawler.Domain.Models;
using HemnetCrawler.Domain;

namespace HemnetCrawler.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinalBidsDataController : ControllerBase
    {
        private readonly FetchFinalBids _fetchFinalBids;
        private readonly DeleteFinalBids _deleteFinalBids;

        public FinalBidsDataController(FetchFinalBids fetchFinalBids, DeleteFinalBids deleteFinalBids)
        {
            _fetchFinalBids = fetchFinalBids;
            _deleteFinalBids = deleteFinalBids;
        }

        [HttpGet("finalBid")]
        public FinalBidOutputModel GetFinalBid(int finalBidId)
        {
            return _fetchFinalBids.GetFinalBid(finalBidId);
        }

        [HttpGet("finalBids")]
        public ItemsPage<FinalBidOutputModel> GetFinalBids(int page, int size, SortDirection sortDirection, string orderByProperty)
        {
            return _fetchFinalBids.ListFinalBids(page, size, sortDirection, orderByProperty);
        }

        [HttpGet("relevantFinalBids")]
        public IActionResult GetRelevantFinalBids(int listingId)
        {
            return Ok(new { finalBids = _fetchFinalBids.ListRelevantFinalBids(listingId) });
        }

        [HttpDelete("deleteFinalBid")]
        public IActionResult DeleteFinalBid(int finalBidId)
        {
            _deleteFinalBids.DeleteFinalBid(finalBidId);

            return Ok();
        }
    }
}
