using System.Collections.Generic;
using HemnetCrawler.Domain.Repositories;
using HemnetCrawler.Domain.Entities;
using System.Linq;

namespace HemnetCrawler.Domain
{
    public class HemnetCrawlerInteractor
    {
        private readonly IListingRepository _listingRepository;
        private readonly IFinalBidRepository _finalBidRepository;

        public HemnetCrawlerInteractor(IListingRepository listingRepository, IFinalBidRepository finalBidRepository)
        {
            _listingRepository = listingRepository;
            _finalBidRepository = finalBidRepository;
        }

        private static bool IsFinalBidAMatch(Listing listing, FinalBid finalBid)
        {
            return (listing.Published < finalBid.SoldDate &&
                    listing.HomeType == finalBid.HomeType &&
                    listing.PostalCode == finalBid.PostalCode &&
                    listing.Street == finalBid.Street);
        }

        public void AddFinalBidToListing()
        {
            List<FinalBid> finalBids = _finalBidRepository.GetAll().OrderBy(fb => fb.SoldDate).ToList();

            foreach (Listing listing in _listingRepository.GetAll().ToList())
            {
                FinalBid match = finalBids.FirstOrDefault(fb => IsFinalBidAMatch(listing, fb));

                if (match != null)
                {
                    listing.FinalBidID = match.Id;
                    _listingRepository.UpdateListing(listing);
                }
            }
        }

        public List<ListingOutputModel> ListListings()
        {
            List<Listing> allListings = _listingRepository.GetAll().Take(100).ToList();

            List<ListingOutputModel> outputModels = allListings.Select(l => new ListingOutputModel(l.Id, l.Street, l.City, l.PostalCode, l.Price, l.Rooms, l.HomeType, l.LivingArea, l.Fee)).ToList();

            return outputModels;
        }

        public List<FinalBidOutputModel> ListFinalBids()
        {
            List<FinalBid> allFinalBids = _finalBidRepository.GetAll().Take(100).ToList();

            List<FinalBidOutputModel> outputModels = allFinalBids.Select(fb => new FinalBidOutputModel(fb.Id, fb.Street, fb.City, fb.PostalCode, fb.Price, fb.SoldDate, fb.DemandedPrice, fb.PriceDevelopment, fb.HomeType, fb.Rooms, fb.LivingArea, fb.Fee)).ToList();

            return outputModels;
        }
    }
}
