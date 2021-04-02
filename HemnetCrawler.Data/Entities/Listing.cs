using System;
using System.Collections.Generic;
using System.Text;

namespace HemnetCrawler.Data.Entities
{
    class Listing
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string HomeType { get; set; }
        public string OwnershipType { get; set; }
        public int Rooms { get; set; }
        public int LivingArea { get; set; }
        public int Fee { get; set; }
        public int BiArea { get; set; }
        public int ConstructionYear { get; set; }
        public int Utilities { get; set; }
        public int Visits { get; set; }
        public int DaysOnHemnet { get; set; }

    }
}
