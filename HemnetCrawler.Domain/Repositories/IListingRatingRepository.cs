using HemnetCrawler.Domain.Entities;
using System.Linq;

namespace HemnetCrawler.Domain.Repositories
{
    public interface IListingRatingRepository
    {
        void AddListingRating(ListingRating rating);
        IQueryable<ListingRating> GetAll();
    }
}
