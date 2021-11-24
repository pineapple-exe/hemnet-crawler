using HemnetCrawler.Domain.Models;
using HemnetCrawler.Domain.Repositories;
using HemnetCrawler.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;

namespace HemnetCrawler.Domain.Interactors
{
    public class FetchListings
    {
        private readonly IListingRepository _listingRepository;

        public FetchListings(IListingRepository listingRepository)
        {
            _listingRepository = listingRepository;
        }

        private static ListingOutputModel MapListingToOutputModel(Listing listing, int[] imageIds)
        {
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

        public ListingOutputModel GetListing(int listingId)
        {
            Listing listing = _listingRepository.GetAllListings().ToList().Find(l => l.Id == listingId);

            if (listing == null) throw new NotFoundException("Listing");

            int[] imageIds = _listingRepository.GetAllImages().Where(img => img.ListingId == listing.Id).Select(img => img.Id).ToArray();

            return MapListingToOutputModel(listing, imageIds);
        }
        public enum Order
        {
            Ascending,
            Descending
        }

        public static List<ListingOutputModel> OrderByStation<T>(List<ListingOutputModel> outputModels, Order order, Func<ListingOutputModel, T> orderByRule)
        {
            CultureInfo culture = new("sv-SE");
            StringComparer stringComparer = StringComparer.Create(culture, false);
            string stringOrderByRule(ListingOutputModel l) => (string)Convert.ChangeType(orderByRule(l), typeof(string));

            if (order == Order.Ascending)
            {
                if (typeof(T) == typeof(string)) 
                    return outputModels.OrderBy(stringOrderByRule, stringComparer).ToList();
                else 
                    return outputModels.OrderBy(orderByRule).ToList();
            }
            else
            {
                if (typeof(T) == typeof(string))
                    return outputModels.OrderByDescending(stringOrderByRule, stringComparer).ToList();
                else
                    return outputModels.OrderByDescending(orderByRule).ToList();
            }
        }

        public static List<ListingOutputModel> OrderListings(List<ListingOutputModel> outputModels, Order order, string by)
        {
            IEnumerable<ListingOutputModel> orderedModels = outputModels;

            return
                by == "id" ? OrderByStation(outputModels, order, l => l.Id) :
                by == "street" ? OrderByStation(outputModels, order, l => l.Street) :
                by == "city" ? OrderByStation(outputModels, order, l => l.City) :
                by == "postal code" ? OrderByStation(outputModels, order, l => l.PostalCode) :
                by == "price" ? OrderByStation(outputModels, order, l => l.Price) :
                by == "rooms" ? OrderByStation(outputModels, order, l => l.Rooms) :
                by == "home type" ? OrderByStation(outputModels, order, l => l.HomeType) :
                by == "living area" ? OrderByStation(outputModels, order, l => l.LivingArea) :
                by == "fee" ? OrderByStation(outputModels, order, l => l.Fee) :
                outputModels;
        }

        public EntitiesPage<ListingOutputModel> ListListings(int page, int size, Order order, string by)
        {
            IQueryable<Listing> allListings = _listingRepository.GetAllListings().Skip(size * page).Take(size);
            IQueryable<Image> images = _listingRepository.GetAllImages();

            int total = _listingRepository.GetAllListings().Count();

            List<ListingOutputModel> outputModels = allListings.Select(l => 
                MapListingToOutputModel(l, images.Where(img => img.ListingId == l.Id).Select(img => img.Id).ToArray())
                ).ToList();

            return new EntitiesPage<ListingOutputModel>(OrderListings(outputModels, order, by), total);
        }
    }
}
