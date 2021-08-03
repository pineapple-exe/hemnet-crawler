using System.Linq;
using HemnetCrawler.Domain.Entities;

namespace HemnetCrawler.Domain.Repositories
{
    public interface IListingRepository
    {
        void AddListing(Listing listing);
        void AddImage(Image image);
        IQueryable<Listing> GetAllListings();
        IQueryable<Image> GetAllImages();
        void UpdateListing(Listing listing);
    }
}
