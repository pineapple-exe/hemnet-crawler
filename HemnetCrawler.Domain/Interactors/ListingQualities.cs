using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Models;
using HemnetCrawler.Domain.Repositories;
using System.Linq;


namespace HemnetCrawler.Domain.Interactors
{
    public class ListingQualities
    {
        private readonly IListingRepository _listingRepository;
        private readonly IFinalBidRepository _finalBidRepository;
        private readonly IListingRatingRepository _listingRatingRepository;

        public ListingQualities(IListingRepository listingRepository, IFinalBidRepository finalBidRepository, IListingRatingRepository listingRatingRepository)
        {
            _listingRepository = listingRepository;
            _finalBidRepository = finalBidRepository;
            _listingRatingRepository = listingRatingRepository;
        }

        public byte[] GetImageData(int imageId)
        {
            byte[] imageData = _listingRepository.GetAllImages().First(img => img.Id == imageId).Data;

            return imageData;
        }

        public double GetAveragePrice(int listingId)
        {
            double averagePrice = FetchFinalBids.FinalBidsThroughRelevanceAlgorithm(listingId, _listingRepository, _finalBidRepository).Select(fb => fb.Price).Average();

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
