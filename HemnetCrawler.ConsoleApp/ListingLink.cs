using System;
using System.Collections.Generic;
using System.Text;

namespace HemnetCrawler.ConsoleApp
{
    public class ListingLink
    {
        public string Href { get; }
        public bool NewConstruction { get; }

        public ListingLink(string href, bool newConstruction)
        {
            Href = href;
            NewConstruction = newConstruction;
        }
    }
}
