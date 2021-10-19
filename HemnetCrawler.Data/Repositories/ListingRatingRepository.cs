using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;
using System.Linq;

namespace HemnetCrawler.Data.Repositories
{
    public class ListingRatingRepository : IListingRatingRepository
    {
        private readonly HemnetCrawlerDbContext _context;

        public ListingRatingRepository(HemnetCrawlerDbContext context)
        {
            _context = context;
        }

        public void AddListingRating(ListingRating rating)
        {
            ListingRating preExistent = _context.ListingRatings.FirstOrDefault(lr => lr.ListingId == rating.ListingId);
            if (preExistent != null)
            {
                preExistent.KitchenRating = rating.KitchenRating;
                preExistent.BathroomRating = rating.BathroomRating;

                _context.Update(preExistent);
            }
            else
            {
                _context.Add(rating);
            }
            _context.SaveChanges();
        }

        public void DeleteListingRating(ListingRating rating)
        {
            _context.Remove(rating);
            _context.SaveChanges();
        }

        public IQueryable<ListingRating> GetAll()
        {
            return _context.ListingRatings;
        }
    }
}
