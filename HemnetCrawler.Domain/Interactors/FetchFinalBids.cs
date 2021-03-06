using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Models;
using HemnetCrawler.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static HemnetCrawler.Domain.Interactors.Utils;

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

        private static FinalBidOutputModel MapFinalBidToOutputModel(FinalBid finalBid, Listing listing)
        {
            return new FinalBidOutputModel
            {
                Id = finalBid.Id,
                ListingId = listing?.Id,
                Street = finalBid.Street,
                City = finalBid.City,
                PostalCode = finalBid.PostalCode,
                Price = finalBid.Price,
                SoldDate = finalBid.SoldDate,
                DemandedPrice = finalBid.DemandedPrice,
                PriceDevelopment = finalBid.PriceDevelopment,
                HomeType = finalBid.HomeType,
                Rooms = finalBid.Rooms,
                LivingArea = finalBid.LivingArea,
                Fee = finalBid.Fee
            };
        }

        public FinalBidOutputModel GetFinalBid(int finalBidId)
        {
            FinalBid finalBid = _finalBidRepository.GetAll().Where(fb => fb.Id == finalBidId).FirstOrDefault();

            if (finalBid == null) throw new NotFoundException("Final Bid");

            Listing listing = _listingRepository.GetAllListings().FirstOrDefault(l => l.FinalBidId == finalBid.Id);

            return MapFinalBidToOutputModel(finalBid, listing);
        }

        internal static IQueryable<FinalBid> ApplyFilter(FinalBidsFilterInputModel filter, IQueryable<FinalBid> unfiltered)
        {
            IQueryable<FinalBid> filtered = unfiltered;

            if (!string.IsNullOrEmpty(filter.HomeType))
            {
                filtered = filtered.Where(l => l.HomeType == filter.HomeType);
            }
            if (filter.RoomsMinimum != null)
            {
                filtered = filtered.Where(l => l.Rooms >= (double)filter.RoomsMinimum);
            }
            if (filter.RoomsMaximum != null)
            {
                filtered = filtered.Where(l => l.Rooms <= (double)filter.RoomsMaximum);
            }
            if (!string.IsNullOrEmpty(filter.Street))
            {
                filtered = filtered.Where(l => l.Street.ToLower().Contains(filter.Street.ToLower()));
            }

            return filtered;
        }

        public ItemsPage<FinalBidOutputModel> ListFinalBids(int pageIndex, int size, FinalBidsFilterInputModel filter, SortDirection sortDirection = SortDirection.Ascending, string orderByProperty = "Id")
        {
            IQueryable<FinalBid> allFilteredFinalBids = ApplyFilter(filter, _finalBidRepository.GetAll());
            List<FinalBid> finalBids = allFilteredFinalBids
                .OrderBy(sortDirection, orderByProperty)
                .Skip(pageIndex * size)
                .Take(size)
                .ToList();

            int total = _finalBidRepository.GetAll().Count();

            List<int> allFinalBidsId = finalBids.Select(fb => fb.Id).ToList();
            List<Listing> connectedListings = _listingRepository.GetAllListings().Where(l => allFinalBidsId.Contains((int)l.FinalBidId)).ToList();

            IEnumerable<FinalBidOutputModel> outputModels = finalBids.Select(fb =>
                MapFinalBidToOutputModel(fb, connectedListings.FirstOrDefault(l => l.FinalBidId == fb.Id))
                );

            return new ItemsPage<FinalBidOutputModel>(outputModels, total);
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
