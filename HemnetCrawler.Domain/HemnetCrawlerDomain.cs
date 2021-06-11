using System;
using System.Collections.Generic;
using System.Text;
using HemnetCrawler.Domain.Repositories;
using HemnetCrawler.Domain.Entities;
using System.Linq;

namespace HemnetCrawler.Domain
{
    public class HemnetCrawlerDomain
    {
        private IListingRepository _listingRepository;
        private IFinalBidRepository _finalBidRepository;

        public HemnetCrawlerDomain(IListingRepository listingRepository, IFinalBidRepository finalBidRepository)
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
    }
}
