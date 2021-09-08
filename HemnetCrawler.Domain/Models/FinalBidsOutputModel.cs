using System.Collections.Generic;

namespace HemnetCrawler.Domain.Models
{
    public class FinalBidsOutputModel
    {
        public List<FinalBidOutputModel> FinalBidsSubset { get; }
        public int Total { get; }

        public FinalBidsOutputModel(List<FinalBidOutputModel> finalBidsSubset, int total)
        {
            FinalBidsSubset = finalBidsSubset;
            Total = total;
        }
    }
}
