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

        public ImageOutputModel GetImage(int imageId)
        {
            Image image = _listingRepository.GetAllImages().FirstOrDefault(img => img.Id == imageId);

            if (image == null)
                throw new NotFoundException("Image");
            else
                return new ImageOutputModel(image.Data, image.ContentType);
        }

        public double? GetEstimatedPrice(int listingId)
        {
            Listing theListing = _listingRepository.GetAllListings().Single(l => l.Id == listingId);
            FinalBid finalBid = _finalBidRepository.GetAll().FirstOrDefault(fb => fb.Id == theListing.FinalBidId);

            if (finalBid!= null) 
                return finalBid.Price;

            IQueryable<FinalBid> relevantFinalBids = FetchFinalBids.FinalBidsThroughRelevanceAlgorithm(listingId, _listingRepository, _finalBidRepository);

            if (relevantFinalBids.Any())
                return relevantFinalBids.Select(fb => fb.Price).Average();
            else
                return null;
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

        public void AddListingRating(int listingId, int? kitchenRating, int? bathroomRating)
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
