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

        private static IEnumerable<FinalBid> OrderFinalBids(IQueryable<FinalBid> finalBids, SortDirection order, string by)
        {
            return
                by == "id" ? OrderByStation(finalBids, order, fb => fb.Id) :
                by == "street" ? OrderByStation(finalBids, order, fb => fb.Street) :
                by == "city" ? OrderByStation(finalBids, order, fb => fb.City) :
                by == "postal code" ? OrderByStation(finalBids, order, fb => fb.PostalCode) :
                by == "price" ? OrderByStation(finalBids, order, fb => fb.Price) :
                by == "rooms" ? OrderByStation(finalBids, order, fb => fb.Rooms) :
                by == "home type" ? OrderByStation(finalBids, order, fb => fb.HomeType) :
                by == "living area" ? OrderByStation(finalBids, order, fb => fb.LivingArea) :
                by == "fee" ? OrderByStation(finalBids, order, fb => fb.Fee) :
                by == "sold date" ? OrderByStation(finalBids, order, fb => fb.SoldDate) :
                by == "demanded price" ? OrderByStation(finalBids, order, fb => fb.DemandedPrice) :
                finalBids;
        }

        public ItemsPage<FinalBidOutputModel> ListFinalBids(int pageIndex, int size, SortDirection order = SortDirection.Ascending, string by = "id")
        {
            IEnumerable<FinalBid> allFinalBids = OrderFinalBids(_finalBidRepository.GetAll(), order, by).Skip(pageIndex * size).Take(size);
            int total = _finalBidRepository.GetAll().Count();

            IEnumerable<FinalBidOutputModel> outputModels = allFinalBids.Select(fb =>
                MapFinalBidToOutputModel(fb, _listingRepository.GetAllListings().FirstOrDefault(l => l.FinalBidId == fb.Id))
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
