using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemnetCrawler.Domain.Tests.Unit.FakeRepositories
{
    class FakeListingRatingRepository : IListingRatingRepository
    {
        public List<ListingRating> listingRatings = new();

        public void AddListingRating(ListingRating rating)
        {
            listingRatings.Add(rating);
        }

        public void DeleteListingRating(ListingRating rating)
        {
            listingRatings.Remove(rating);
        }

        public IQueryable<ListingRating> GetAll()
        {
            return listingRatings.AsQueryable();
        }
    }
}
