
namespace HemnetCrawler.Domain.Models
{
    public class FinalBidsFilterInputModel
    {
        public string HomeType { get; init; }
        public int? RoomsMinimum { get; init; }
        public int? RoomsMaximum { get; init; }
        public string Street { get; init; }
    }
}
