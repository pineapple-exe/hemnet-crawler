namespace HemnetCrawler.Domain.Models
{
    public class ListingRatingOutputModel
    {
        public int? KitchenRating { get; }
        public int? BathroomRating { get; }

        public ListingRatingOutputModel(int? kitchenRating = null, int? bathroomRating = null)
        {
            KitchenRating = kitchenRating;
            BathroomRating = bathroomRating;
        }
    }
}
