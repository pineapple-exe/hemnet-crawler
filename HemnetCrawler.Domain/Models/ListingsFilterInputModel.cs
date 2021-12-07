using HemnetCrawler.Domain.Entities;
using System.Linq;
using System.Text.RegularExpressions;

namespace HemnetCrawler.Domain.Models
{
    public class ListingsFilterInputModel
    {
        public string HomeType { get; set; }
        public int? RoomsMinimum { get; set; }
        public int? RoomsMaximum { get; set; }
        public string Street { get; set; }

        internal IQueryable<Listing> ApplyFilter(IQueryable<Listing> unfiltered)
        {
            IQueryable<Listing> filtered = unfiltered;

            if (!string.IsNullOrEmpty(HomeType))
            {
                filtered = filtered.Where(l => l.HomeType == HomeType);
            }
            if (RoomsMinimum != null)
            {
                var roomsMinimum = RoomsMinimum.ToString();
                filtered = filtered.Where(l => l.Rooms == roomsMinimum);
            }
            if (RoomsMaximum != null)
            {
                var roomsMaximum = RoomsMinimum.ToString();
                filtered = filtered.Where(l => l.Rooms == roomsMaximum);
            }
            if (!string.IsNullOrEmpty(Street))
            {
                filtered = filtered.Where(l => l.Street.ToLower() == Street.ToLower()); //testa sedan att addera Contains
            }

            return filtered;
        }
    }
}
