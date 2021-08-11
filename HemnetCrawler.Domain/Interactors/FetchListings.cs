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

        public List<ListingOutputModel> ListListings()
        {
            IQueryable<Listing> allListings = _listingRepository.GetAllListings().Take(100);
            IQueryable<Image> images = _listingRepository.GetAllImages();

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
                ImageIds = images.Where(img => img.ListingId == l.Id).Select(img => img.Id).ToArray()
            }).ToList();

            return outputModels;
        }
    }
}
