using System.Collections.Generic;
using HemnetCrawler.Domain.Repositories;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Models;
using System.Linq;

namespace HemnetCrawler.Domain
{
    public class HemnetCrawlerInteractor
    {
        private readonly IListingRepository _listingRepository;
        private readonly IFinalBidRepository _finalBidRepository;
        private readonly IListingRatingRepository _listingRatingRepository;

        public HemnetCrawlerInteractor(IListingRepository listingRepository, IFinalBidRepository finalBidRepository, IListingRatingRepository listingRatingRepository)
        {
            _listingRepository = listingRepository;
            _finalBidRepository = finalBidRepository;
            _listingRatingRepository = listingRatingRepository;
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
            var finalBids = _finalBidRepository.GetAll().OrderBy(fb => fb.SoldDate).ToList();

            foreach (Listing listing in _listingRepository.GetAllListings())
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
            var allListings = _listingRepository.GetAllListings().Take(100);
            IQueryable<Image> images = _listingRepository.GetAllImages();

            List<ListingOutputModel> outputModels = allListings.Select(l => new ListingOutputModel(l.Id, l.Street, l.City, l.PostalCode, l.Price, l.Rooms, l.HomeType, l.LivingArea, l.Fee, images.Where(img => img.ListingId == l.Id).Select(img => img.Id).ToArray())).ToList();

            return outputModels;
        }

        public byte[] GetImageData(int imageId)
        {
            byte[] imageData = _listingRepository.GetAllImages().First(img => img.Id == imageId).Data;

            return imageData;
        }

        public List<FinalBidOutputModel> ListFinalBids()
        {
            var allFinalBids = _finalBidRepository.GetAll().Take(100);

            List<FinalBidOutputModel> outputModels = allFinalBids.Select(fb => new FinalBidOutputModel(fb.Id, fb.Street, fb.City, fb.PostalCode, fb.Price, fb.SoldDate, fb.DemandedPrice, fb.PriceDevelopment, fb.HomeType, fb.Rooms, fb.LivingArea, fb.Fee)).ToList();

            return outputModels;
        }

        public double GetAveragePrice(int listingId)
        {
            Listing objectOfSpeculation = _listingRepository.GetAllListings().First(l => l.Id == listingId);

            var relevantFinalBids = _finalBidRepository.GetAll().Where(fb => fb.HomeType == objectOfSpeculation.HomeType);

            if (objectOfSpeculation.Rooms != null && relevantFinalBids.Where(fb => fb.Rooms == objectOfSpeculation.Rooms).Count() > 0)
            {
                relevantFinalBids = relevantFinalBids.Where(fb => fb.Rooms == objectOfSpeculation.Rooms);
            }

            if (relevantFinalBids.Where(fb => fb.City == objectOfSpeculation.City).Count() > 0)
            { 
                relevantFinalBids = relevantFinalBids.Where(fb => fb.City == objectOfSpeculation.City);
            }

            if (objectOfSpeculation.PostalCode != null && relevantFinalBids.Where(fb => fb.PostalCode == objectOfSpeculation.PostalCode).Count() > 0)
            {
                relevantFinalBids = relevantFinalBids.Where(fb => fb.PostalCode == objectOfSpeculation.PostalCode);                                      
            }

            double averagePrice = relevantFinalBids.Select(fb => fb.Price).Average();

            return averagePrice;
        }

        public ListingRatingOutputModel GetListingRating(int listingId)
        {
            ListingRating relevantListingRating = _listingRatingRepository.GetAll().FirstOrDefault(lr => lr.ListingId == listingId);

            if (relevantListingRating != null)
            { 
                return new ListingRatingOutputModel(relevantListingRating.KitchenRating, relevantListingRating.BathroomRating);
            }
            else
            {
                return new ListingRatingOutputModel();
            }
        }

        public void AddListingRating(int listingId, int kitchenRating, int bathroomRating)
        {
            ListingRating listingRating = new ListingRating
            {
                ListingId = listingId,
                KitchenRating = kitchenRating,
                BathroomRating = bathroomRating
            };

            _listingRatingRepository.AddListingRating(listingRating);
        }
    }
}
