using System;

namespace HemnetCrawler.Domain.Entities
{
    public class Image : ICloneable
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
        public Listing Listing { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
