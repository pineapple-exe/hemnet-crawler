using System;

namespace HemnetCrawler.Domain
{
    public class FinalBidOutputModel
    {
        public int Id { get; }
        public string Street { get; }
        public string City { get; }
        public int? PostalCode { get; }
        public int Price { get; }
        public DateTimeOffset SoldDate { get; }
        public int DemandedPrice { get; }
        public string PriceDevelopment { get; }
        public string HomeType { get; }
        public string Rooms { get; }
        public double? LivingArea { get; }
        public int? Fee { get; }

        public FinalBidOutputModel(int id, string street, string city, int? postalCode, int price, DateTimeOffset soldDate, int demandedPrice, string priceDevelopment, string homeType, string rooms, double? livingArea, int? fee)
        {
            Id = id;
            Street = street;
            City = city;
            PostalCode = postalCode;
            Price = price;
            SoldDate = soldDate;
            DemandedPrice = demandedPrice;
            PriceDevelopment = priceDevelopment;
            HomeType = homeType;
            Rooms = rooms;
            LivingArea = livingArea;
            Fee = fee;
        }
    }
}
