using System;
using System.Collections.Generic;
using System.Linq;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;

namespace HemnetCrawler.Domain.Tests.Unit
{
    class FakeListingRepository : IListingRepository
    {
        public List<Listing> Listings = new();
        public List<Image> Images = new();

        public void AddImage(Image image)
        {
            Images.Add((Image)image.Clone());
        }

        public void AddListing(Listing listing)
        {
            Listings.Add((Listing)listing.Clone());
        }

        public IQueryable<Image> GetAllImages()
        {
            return Images.Select(i => (Image)i.Clone()).AsQueryable();
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
