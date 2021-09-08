using System.Collections.Generic;

namespace HemnetCrawler.Domain.Models
{
    public class PaginatedListingsOutputModel
    {
        public List<ListingOutputModel> ListingsSubset { get; }
        public int Total { get; }

        public PaginatedListingsOutputModel(List<ListingOutputModel> listingsSubset, int total)
        {
            ListingsSubset = listingsSubset;
            Total = total;
        }
    }
}
