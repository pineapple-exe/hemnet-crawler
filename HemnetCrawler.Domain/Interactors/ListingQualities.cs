using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Models;
using HemnetCrawler.Domain.Repositories;
using System.Linq;


namespace HemnetCrawler.Domain
{
    public class ListingQualities
    {
        private readonly IListingRepository _listingRepository;
        private readonly IFinalBidRepository _finalBidRepository;
        private readonly IListingRatingRepository _listingRatingRepository;

        public byte[] GetImageData(int imageId)
        {
            byte[] imageData = _listingRepository.GetAllImages().First(img => img.Id == imageId).Data;

            return imageData;
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
            ListingRating listingRating = new()
            {
                ListingId = listingId,
                KitchenRating = kitchenRating,
                BathroomRating = bathroomRating
            };

            _listingRatingRepository.AddListingRating(listingRating);
        }
    }
}
