using System.ComponentModel.DataAnnotations;

namespace HemnetCrawler.Domain.Entities
{
    public class ListingRating
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        [Range(0, 2)]
        public int? KitchenRating { get; set; }
        [Range(0, 2)]
        public int? BathroomRating { get; set; }
        public Listing Listing { get; set; }
    }
}
