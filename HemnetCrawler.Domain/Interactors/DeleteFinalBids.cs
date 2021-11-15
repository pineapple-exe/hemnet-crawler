﻿using HemnetCrawler.Domain.Repositories;
using HemnetCrawler.Domain.Entities;
using System.Linq;
using System.Collections.Generic;

namespace HemnetCrawler.Domain.Interactors
{
    public class DeleteFinalBids
    {
        private readonly IFinalBidRepository _finalBidRepository;
        private readonly IListingRepository _listingRepository;

        public DeleteFinalBids(IFinalBidRepository finalBidRepository, IListingRepository listingRepository)
        {
            _finalBidRepository = finalBidRepository;
            _listingRepository = listingRepository;
        }

        public void DeleteFinalBid(int finalBidId)
        {
            List<Listing> listingsToBeAltered = _listingRepository.GetAllListings().Where(l => l.FinalBidId == finalBidId).ToList();

            if (listingsToBeAltered.Any())
            {
                foreach (Listing listing in listingsToBeAltered)
                {
                    listing.FinalBidId = null;
                    _listingRepository.UpdateListing(listing);
                }
            }

            FinalBid finalBidToBeDeleted = _finalBidRepository.GetAll().SingleOrDefault(l => l.Id == finalBidId);

            if (finalBidToBeDeleted == null)
                throw new NotFoundException("Final bid to be deleted not found.");
            else
                _finalBidRepository.DeleteFinalBid(finalBidToBeDeleted);
        }
    }
}
