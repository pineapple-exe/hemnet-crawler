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
                filtered = filtered.Where(l => l.Rooms >= (double)RoomsMinimum);
            }
            if (RoomsMaximum != null)
            {
                filtered = filtered.Where(l => l.Rooms <= (double)RoomsMaximum);
            }
            if (!string.IsNullOrEmpty(Street))
            {
                filtered = filtered.Where(l => l.Street.ToLower().Contains(Street.ToLower()));
            }

            return filtered;
        }
    }
}
