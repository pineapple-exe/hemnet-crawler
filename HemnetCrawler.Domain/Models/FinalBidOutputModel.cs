using System;

namespace HemnetCrawler.Domain.Models
{
    public class FinalBidOutputModel
    {
        public int Id { get; init; }
        public string Street { get; init; }
        public string City { get; init; }
        public int? PostalCode { get; init; }
        public int Price { get; init; }
        public DateTimeOffset SoldDate { get; init; }
        public int DemandedPrice { get; init; }
        public string PriceDevelopment { get; init; }
        public string HomeType { get; init; }
        public string Rooms { get; init; }
        public double? LivingArea { get; init; }
        public int? Fee { get; init; }
        public int? ListingId { get; init; }
        public bool? HasRating { get; init; }
    }
}
