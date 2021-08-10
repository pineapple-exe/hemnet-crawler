using System;
using System.Collections.Generic;
using System.Linq;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;

namespace HemnetCrawler.Domain.Tests.Unit
{
    class FakeListingRepository : IListingRepository
    {
        readonly List<Listing> listings = new()
        {
            new Listing()
            {
                Id = 729,
                PostalCode = 41760,
                FinalBidID = 4487,
                HomeType = "Lägenhet"
            },
            new Listing()
            {
                Id = 2649,
                PostalCode = 41760,
                HomeType = "Lägenhet"
            }

        };
        public void AddImage(Image image)
        {
            throw new NotImplementedException();
        }

        public void AddListing(Listing listing)
        {
            listings.Add(listing);
        }

        public IQueryable<Image> GetAllImages()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Listing> GetAllListings()
        {
            return listings.AsQueryable();
        }

        public void UpdateListing(Listing listing)
        {
            throw new NotImplementedException();
        }
    }
}
