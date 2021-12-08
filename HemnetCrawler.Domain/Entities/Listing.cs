using System;

namespace HemnetCrawler.Domain.Entities
{
    public class Listing : ICloneable
    {
        public int Id { get; set; }
        public int HemnetId { get; set; }
        public string Href { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public bool NewConstruction { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int? PostalCode { get; set; }
        public int? Price { get; set; }
        public int? PricePerSquareMeter { get; set; }
        public string Description { get; set; }
        public string HomeType { get; set; }
        public string OwnershipType { get; set; }
        public double? Rooms { get; set; }
        public bool Balcony { get; set; }
        public string Floor { get; set; }
        public double? LivingArea { get; set; }
        public double? BiArea { get; set; }
        public int PropertyArea { get; set; }
        public int? Fee { get; set; }
        public string ConstructionYear { get; set; }
        public string HomeOwnersAssociation { get; set; }
        public int? Utilities { get; set; }
        public string EnergyClassification { get; set; }
        public int Visits { get; set; }
        public DateTime Published { get; set; }
        public int? FinalBidId { get; set; }
        public FinalBid FinalBid { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
