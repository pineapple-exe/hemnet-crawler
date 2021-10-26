using HemnetCrawler.Domain.Repositories;
using HemnetCrawler.Domain.Entities;
using System.Linq;
using System.Collections.Generic;

namespace HemnetCrawler.Domain.Interactors
{
    public class DeleteListings
    {
        private readonly IListingRepository _listingRepository;
        private readonly IListingRatingRepository _listingRatingRepository;

        public DeleteListings(IListingRepository listingRepository, IListingRatingRepository listingRatingRepository)
        {
            _listingRepository = listingRepository;
            _listingRatingRepository = listingRatingRepository;
        }

        public void DeleteListing(int listingId)
        {
            Listing listingToBeDeleted = _listingRepository.GetAllListings().SingleOrDefault(l => l.Id == listingId);
            ListingRating listingRatingToBeDeleted = _listingRatingRepository.GetAll().SingleOrDefault(lr => lr.ListingId == listingId);
            List<Image> imagesToBeDeleted = _listingRepository.GetAllImages().Where(img => img.ListingId == listingId).ToList();

            if (listingRatingToBeDeleted != null) _listingRatingRepository.DeleteListingRating(listingRatingToBeDeleted);
            if (imagesToBeDeleted != null) _listingRepository.DeleteImages(imagesToBeDeleted);

            if (listingToBeDeleted == null) 
                throw new NotFoundException("Listing to be deleted not found."); 
            else 
                _listingRepository.DeleteListing(listingToBeDeleted);
        }
    }
}
