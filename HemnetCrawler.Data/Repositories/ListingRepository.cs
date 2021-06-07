using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;

namespace HemnetCrawler.Data.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private HemnetCrawlerDbContext _context;

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

        public bool IsFinalBidAMatch(Listing listing, FinalBid finalBid)
        {
            return (listing.Published < finalBid.SoldDate &&
                    listing.HomeType == finalBid.HomeType &&
                    listing.PostalCode == finalBid.PostalCode &&
                    listing.Street == finalBid.Street);
        }

        public void AddFinalBidToListing()
        {
            List<FinalBid> finalBids = _context.FinalBids.OrderBy(fb => fb.SoldDate).ToList();

            foreach (Listing listing in _context.Listings)
            {
                FinalBid match = finalBids.FirstOrDefault(fb => IsFinalBidAMatch(listing, fb));

                if (match != null)
                {
                    listing.FinalBidID = match.Id;
                    _context.Update(listing);
                }
            }
            _context.SaveChanges();
        }

        public void DisposeContext()
        {
            _context.Dispose();
        }
    }
}
