namespace HemnetCrawler.Domain.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
        public Listing Listing { get; set; }
    }
}
