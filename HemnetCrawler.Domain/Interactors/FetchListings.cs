using HemnetCrawler.Domain.Models;
using HemnetCrawler.Domain.Repositories;
using HemnetCrawler.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace HemnetCrawler.Domain.Interactors
{
    public class FetchListings
    {
        private readonly IListingRepository _listingRepository;

        public FetchListings(IListingRepository listingRepository)
        {
            _listingRepository = listingRepository;
        }

        public ListingOutputModel GetListing(int listingId)
        {
            Listing listing = _listingRepository.GetAllListings().ToList().Find(l => l.Id == listingId);
            int[] imageIds = _listingRepository.GetAllImages().Where(img => img.ListingId == listing.Id).Select(img => img.Id).ToArray();

            return new ListingOutputModel 
            {   
                Id = listing.Id,
                Street = listing.Street,
                City = listing.City,
                PostalCode = listing.PostalCode,
                Price = listing.Price,
                Rooms = listing.Rooms,
                HomeType = listing.HomeType,
                LivingArea = listing.LivingArea,
                Fee = listing.Fee,
                ImageIds = imageIds,
                FinalBidId = listing.FinalBidId
            };
        }

        public PaginatedListingsOutputModel ListListings(int page, int size)
        {
            IQueryable<Listing> allListings = _listingRepository.GetAllListings().Skip(size * page).Take(size);
            IQueryable<Image> images = _listingRepository.GetAllImages();
            int total = _listingRepository.GetAllListings().Count();

            List<ListingOutputModel> outputModels = allListings.Select(l => new ListingOutputModel
            {
                Id = l.Id,
                Street = l.Street,
                City = l.City,
                PostalCode = l.PostalCode,
                Price = l.Price,
                Rooms = l.Rooms,
                HomeType = l.HomeType,
                LivingArea = l.LivingArea,
                Fee = l.Fee,
                ImageIds = images.Where(img => img.ListingId == l.Id).Select(img => img.Id).ToArray(),
                FinalBidId = l.FinalBidId
            }).ToList();

            return new PaginatedListingsOutputModel(outputModels, total);
        }
    }
}
