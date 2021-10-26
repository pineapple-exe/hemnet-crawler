using HemnetCrawler.Domain.Entities;
using System.Linq;

namespace HemnetCrawler.Domain.Repositories
{
    public interface IListingRatingRepository
    {
        void AddListingRating(ListingRating rating);
        void DeleteListingRating(ListingRating rating);
        IQueryable<ListingRating> GetAll();
    }
}
