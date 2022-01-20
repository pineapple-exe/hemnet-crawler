
namespace HemnetCrawler.Domain.Models
{
    public class ListingsFilterInputModel
    {
        public string HomeType { get; init; }
        public int? RoomsMinimum { get; init; }
        public int? RoomsMaximum { get; init; }
        public string Street { get; init; }
    }
}
