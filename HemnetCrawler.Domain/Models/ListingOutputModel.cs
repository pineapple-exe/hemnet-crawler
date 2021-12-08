

namespace HemnetCrawler.Domain.Models
{
    public class ListingOutputModel
    {
        public int Id { get; init; }
        public string Street { get; init; }
        public string City { get; init; }
        public int? PostalCode { get; init; }
        public int? Price { get; init; }
        public double? Rooms { get; init; }
        public string HomeType { get; init; }
        public double? LivingArea { get; init; }
        public int? Fee { get; init; }
        public int[] ImageIds { get; init; }
        public int? FinalBidId { get; init; }
    }
}
