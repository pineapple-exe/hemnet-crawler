using System.Collections.Generic;

namespace HemnetCrawler.Domain.Models
{
    public class ListingsOutputModel
    {
        public List<ListingOutputModel> ListingsSubset { get; }
        public int Total { get; }

        public ListingsOutputModel(List<ListingOutputModel> listingsSubset, int total)
        {
            ListingsSubset = listingsSubset;
            Total = total;
        }
    }
}
