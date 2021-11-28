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
        public enum SortDirection
        {
            Ascending,
            Descending
        }

        public static List<Listing> OrderByStation<T>(IQueryable<Listing> listings, SortDirection order, Func<Listing, T> orderByRule)
        {
            CultureInfo culture = new("sv-SE");
            StringComparer stringComparer = StringComparer.Create(culture, false);
            string stringOrderByRule(Listing l) => (string)Convert.ChangeType(orderByRule(l), typeof(string));

            if (order == SortDirection.Ascending)
            {
                if (typeof(T) == typeof(string)) 
                    return listings.OrderBy(stringOrderByRule, stringComparer).ToList();
                else 
                    return listings.OrderBy(orderByRule).ToList();
            }
            else
            {
                if (typeof(T) == typeof(string))
                    return listings.OrderByDescending(stringOrderByRule, stringComparer).ToList();
                else
                    return listings.OrderByDescending(orderByRule).ToList();
            }
        }

        public static List<Listing> OrderListings(IQueryable<Listing> listings, SortDirection order, string by)
        {
            return
                by == "id" ? OrderByStation(listings, order, l => l.Id) :
                by == "street" ? OrderByStation(listings, order, l => l.Street) :
                by == "city" ? OrderByStation(listings, order, l => l.City) :
                by == "postal code" ? OrderByStation(listings, order, l => l.PostalCode) :
                by == "price" ? OrderByStation(listings, order, l => l.Price) :
                by == "rooms" ? OrderByStation(listings, order, l => l.Rooms) :
                by == "home type" ? OrderByStation(listings, order, l => l.HomeType) :
                by == "living area" ? OrderByStation(listings, order, l => l.LivingArea) :
                by == "fee" ? OrderByStation(listings, order, l => l.Fee) :
                listings.ToList();
        }

        public ItemsPage<ListingOutputModel> ListListings(int pageIndex, int size, SortDirection order = SortDirection.Ascending, string by = "id")
        {
            IEnumerable<Listing> allListings = OrderListings(_listingRepository.GetAllListings(), order, by).Skip(size * pageIndex).Take(size);
            IQueryable<Image> images = _listingRepository.GetAllImages();

            int total = _listingRepository.GetAllListings().Count();

            List<ListingOutputModel> outputModels = allListings.Select(l => 
                MapListingToOutputModel(l, images.Where(img => img.ListingId == l.Id).Select(img => img.Id).ToArray())
                ).ToList();

            return new ItemsPage<ListingOutputModel>(outputModels, total);
        }
    }
}
