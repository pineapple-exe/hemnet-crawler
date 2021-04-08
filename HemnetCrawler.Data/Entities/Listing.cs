using System;
using System.Collections.Generic;
using System.Text;

namespace HemnetCrawler.Data.Entities
{
    public class Listing
    {
        public int Id { get; set; }
        public int HemnetId { get; set; }
        public bool NewConstruction { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int? Price { get; set; }
        public int? PricePerSquareMeter { get; set; }
        public string Description { get; set; }
        public string HomeType { get; set; }
        public string OwnershipType { get; set; }
        public int? Rooms { get; set; }
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
        public int DaysOnHemnet { get; set; }
    }
}
