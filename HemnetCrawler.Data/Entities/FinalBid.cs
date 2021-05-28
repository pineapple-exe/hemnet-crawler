using System;

namespace HemnetCrawler.Data.Entities
{
    public class FinalBid
    {
        public int Id { get; set; }
        public int HemnetId { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int? PostalCode { get; set; }
        public int Price { get; set; }
        public DateTimeOffset SoldDate { get; set; }
        public int DemandedPrice { get; set; }
        public string PriceDevelopment { get; set; }
        public int? PricePerSquareMeter { get; set; }
        public string HomeType { get; set; }
        public string OwnershipType { get; set; }
        public string Rooms { get; set; }
        public double? LivingArea { get; set; }
        public int? Fee { get; set; }
        public string ConstructionYear { get; set; }
        public int? LandLeaseFee { get; set; }
        public int? Utilities { get; set; }
    }
}
