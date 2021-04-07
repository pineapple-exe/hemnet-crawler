using System;
using System.Collections.Generic;
using System.Text;

namespace HemnetCrawler.ConsoleApp
{
    public class ListingLink
    {
        public int Id { get; }
        public string Href { get; }
        public bool NewConstruction { get; }

        public ListingLink(int id, string href, bool newConstruction)
        {
            Id = id;
            Href = href;
            NewConstruction = newConstruction;
        }
    }
}
