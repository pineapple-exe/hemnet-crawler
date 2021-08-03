using System.Linq;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;

namespace HemnetCrawler.Data.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly HemnetCrawlerDbContext _context;

        public ListingRepository(HemnetCrawlerDbContext context)
        {
            _context = context;
        }

        public void AddListing(Listing listing)
        {
            _context.Add(listing);
            _context.SaveChanges();
        }

        public void AddImage(Image image)
        {
            _context.Add(image);
            _context.SaveChanges();
        }

        public void UpdateListing(Listing listing)
        {
            _context.Update(listing);
            _context.SaveChanges();
        }

        public IQueryable<Listing> GetAllListings()
        {
            return _context.Listings;
        }

        public IQueryable<Image> GetAllImages()
        {
            return _context.Images;
        }
    }
}
