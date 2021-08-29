﻿using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Models;
using HemnetCrawler.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace HemnetCrawler.Domain.Interactors
{
    public class FetchFinalBids
    {
        private readonly IFinalBidRepository _finalBidRepository;
        private readonly IListingRepository _listingRepository;
        private readonly IListingRatingRepository _listingRatingRepository;

        public FetchFinalBids(IFinalBidRepository finalBidRepository, IListingRepository listingRepository, IListingRatingRepository listingRatingRepository)
        {
            _finalBidRepository = finalBidRepository;
            _listingRepository = listingRepository;
            _listingRatingRepository = listingRatingRepository;
        }

        public List<FinalBidOutputModel> ListFinalBids()
        {
            IQueryable<FinalBid> allFinalBids = _finalBidRepository.GetAll();

            List<FinalBidOutputModel> outputModels = allFinalBids.Select(fb => new FinalBidOutputModel
            {
                Id = fb.Id,
                ListingId = _listingRepository.GetAllListings().Where(l => l.FinalBidId == fb.Id).Select(l => l.Id).FirstOrDefault(),
                Street = fb.Street,
                City = fb.City,
                PostalCode = fb.PostalCode,
                Price = fb.Price,
                SoldDate = fb.SoldDate,
                DemandedPrice = fb.DemandedPrice,
                PriceDevelopment = fb.PriceDevelopment,
                HomeType = fb.HomeType,
                Rooms = fb.Rooms,
                LivingArea = fb.LivingArea,
                Fee = fb.Fee
            }).ToList();

            return outputModels;
        }

        public static IQueryable<FinalBid> FinalBidsThroughRelevanceAlgorithm(int listingId, IListingRepository listingRepository, IFinalBidRepository finalBidRepository)
        {
            Listing listing = listingRepository.GetAllListings().First(l => l.Id == listingId);

            IQueryable<FinalBid> relevantFinalBids = finalBidRepository.GetAll().Where(fb => fb.HomeType == listing.HomeType);

            if (listing.Rooms != null && relevantFinalBids.Any(fb => fb.Rooms == listing.Rooms))
            {
                relevantFinalBids = relevantFinalBids.Where(fb => fb.Rooms == listing.Rooms);
            }

            if (relevantFinalBids.Any(fb => fb.City == listing.City))
            {
                relevantFinalBids = relevantFinalBids.Where(fb => fb.City == listing.City);
            }

            if (listing.PostalCode != null && relevantFinalBids.Any(fb => fb.PostalCode == listing.PostalCode))
            {
                relevantFinalBids = relevantFinalBids.Where(fb => fb.PostalCode == listing.PostalCode);
            }

            return relevantFinalBids;
        }

        public static int? MakeZeroNull(int zeroOrNot)
        {
            return zeroOrNot == 0 ? null : zeroOrNot;
        }

        public List<FinalBidEstimationOutputModel> ListRelevantFinalBids(int listingId)
        {
            var relevantFinalBids = FinalBidsThroughRelevanceAlgorithm(listingId, _listingRepository, _finalBidRepository).Take(100).ToList();
            IEnumerable<int> relevantFinalBidIds = relevantFinalBids.Select(fb => fb.Id);

            var relevantOldListings = _listingRepository.GetAllListings().Where(l => relevantFinalBidIds.Contains(l.Id)).ToList();
            IEnumerable<int> relevantOldListingsIds = relevantOldListings.Select(l => l.Id);

            var relevantRatings = _listingRatingRepository.GetAll().Where(r => relevantOldListingsIds.Contains(r.ListingId)).ToList();

            List<FinalBidEstimationOutputModel> output = relevantFinalBids.Select(fb => new FinalBidEstimationOutputModel
            {
                Id = fb.Id,
                Street = fb.Street,
                City = fb.City,
                PostalCode = fb.PostalCode,
                Price = fb.Price,
                SoldDate = fb.SoldDate,
                DemandedPrice = fb.DemandedPrice,
                PriceDevelopment = fb.PriceDevelopment,
                HomeType = fb.HomeType,
                Rooms = fb.Rooms,
                LivingArea = fb.LivingArea,
                Fee = fb.Fee,
                ListingId = MakeZeroNull(relevantOldListings.Where(l => l.FinalBid == fb).Select(l => l.Id).FirstOrDefault()),
                HasRating = relevantOldListings.Any(l => l.FinalBid == fb && relevantRatings.Any(r => r.Listing == l))
            }).ToList();

            return output;
        }
    }
}
