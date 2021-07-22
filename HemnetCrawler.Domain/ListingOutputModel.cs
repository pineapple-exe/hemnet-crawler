﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HemnetCrawler.Domain
{
    public class ListingOutputModel
    {
        public int Id { get; }
        public string Street { get; }
        public string City { get; }
        public int? PostalCode { get; }
        public int? Price { get; }
        public string Rooms { get; }
        public string HomeType { get; }
        public double? LivingArea { get; }
        public int? Fee { get; }

        public ListingOutputModel(int id, string street, string city, int? postalCode, int? price, string rooms, string homeType, double? livingArea, int? fee)
        {
            Id = id;
            Street = street;
            City = city;
            PostalCode = postalCode;
            Price = price;
            Rooms = rooms;
            HomeType = homeType;
            LivingArea = livingArea;
            Fee = fee;
        }
    }
}