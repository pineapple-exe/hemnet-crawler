﻿using System;
using System.Collections.Generic;
using System.Linq;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;

namespace HemnetCrawler.Domain.Tests.Unit
{
    class FakeListingRepository : IListingRepository
    {
        public List<Listing> Listings = new();

        public void AddImage(Image image)
        {
            throw new NotImplementedException();
        }

        public void AddListing(Listing listing)
        {
            Listings.Add((Listing)listing.Clone());
        }

        public IQueryable<Image> GetAllImages()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Listing> GetAllListings()
        {
            return Listings.Select(l => (Listing)l.Clone()).AsQueryable();
        }

        public void UpdateListing(Listing listing)
        {
            Listings.RemoveAll(l => l.Id == listing.Id);
            Listings.Add((Listing)listing.Clone());
        }
    }
}
