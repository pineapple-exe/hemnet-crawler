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

        internal static IQueryable<Listing> ApplyFilter(ListingsFilterInputModel filter, IQueryable<Listing> unfiltered)
        {
            IQueryable<Listing> filtered = unfiltered;

            if (!string.IsNullOrEmpty(filter.HomeType))
            {
                filtered = filtered.Where(l => l.HomeType == filter.HomeType);
            }
            if (filter.RoomsMinimum != null)
            {
                filtered = filtered.Where(l => l.Rooms >= (double)filter.RoomsMinimum);
            }
            if (filter.RoomsMaximum != null)
            {
                filtered = filtered.Where(l => l.Rooms <= (double)filter.RoomsMaximum);
            }
            if (!string.IsNullOrEmpty(filter.Street))
            {
                filtered = filtered.Where(l => l.Street.ToLower().Contains(filter.Street.ToLower()));
            }

            return filtered;
        }

        public ItemsPage<ListingOutputModel> ListListings(int pageIndex, int size, ListingsFilterInputModel filter, SortDirection sortDirection = SortDirection.Ascending, string orderByProperty = "Id")
        {
            IQueryable<Listing> allFilteredListings = ApplyFilter(filter, _listingRepository.GetAllListings());

            List<Listing> listings = allFilteredListings
                .OrderBy(sortDirection, orderByProperty)
                .Skip(size * pageIndex)
                .Take(size)
                .ToList();
            List<int> listingIds = listings.Select(l => l.Id).ToList();
            List<Image> images = _listingRepository.GetAllImages().Where(img => listingIds.Contains(img.ListingId)).ToList();

            IEnumerable<ListingOutputModel> outputModels = listings.Select(l => 
                MapListingToOutputModel(l, images.Where(img => img.ListingId == l.Id).Select(img => img.Id).ToArray())
                );

            int total = allFilteredListings.Count();

            return new ItemsPage<ListingOutputModel>(outputModels, total);
        }
    }
}
