namespace HemnetCrawler.Domain.Models
{
    public class ListingRatingInputModel
    {
        public int ListingId { get; }
        public int? KitchenRating { get; }
        public int? BathroomRating { get; }
        public ListingRatingInputModel(int listingId, int? kitchenRating, int? bathroomRating)
        {
            ListingId = listingId;
            KitchenRating = kitchenRating;
            BathroomRating = bathroomRating;
        }
    }
}
