using HemnetCrawler.Domain.Entities;

namespace HemnetCrawler.Domain
{
    public class EvaluatedFinalBidMatch
    {
        public int FinalBidId { get; }
        public double PrecisionRate { get; }

        public EvaluatedFinalBidMatch(int finalBidId, double precisionRate)
        {
            FinalBidId = finalBidId;
            PrecisionRate = precisionRate;
        }
    }
}
