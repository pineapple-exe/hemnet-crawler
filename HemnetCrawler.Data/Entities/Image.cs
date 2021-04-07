using System;
using System.Collections.Generic;
using System.Text;

namespace HemnetCrawler.Data.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public int ListingID { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
        public Listing Listing { get; set; }
    }
}
