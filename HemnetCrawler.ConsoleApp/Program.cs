using HemnetCrawler.Data;
using HemnetCrawler.Data.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HemnetCrawler.ConsoleApp.PageInteractives
{
    class Program
    { 
        static void Main(string[] args)
        {
            SearchGatherListings();
            SearchGatherFinalBids();

            AddFinalBidsToListings();
        }

        static void GeneralSearchAndGather(Action<IWebDriver> orderSearchResults, Action<IWebDriver> addAgeFilter, Action<IWebDriver> leafThroughPagesAndCreateRecords)
        {
            ChromeDriver driver = new ChromeDriver();
            
            StartPage.EnterHemnet(driver);
            StartPage.AddSearchBase(driver);
            orderSearchResults(driver);
            addAgeFilter(driver);
            leafThroughPagesAndCreateRecords(driver);

            driver.Quit();
            driver.Dispose();
        }

        static void SearchGatherListings()
        {
            GeneralSearchAndGather(ListingsSearchResults.SortSearchResults, ListingsSearchResults.AddAgeFilter, Mixed.LeafThroughListingPagesAndCreateRecords);
        }

        static void SearchGatherFinalBids()
        {
            GeneralSearchAndGather(FinalBidsSearchResults.SpecifyAndSortResults, FinalBidsSearchResults.AddAgeFilter, Mixed.LeafThroughFinalBidPagesAndCreateRecords);
        }

        static void AddFinalBidsToListings()
        {
            HemnetCrawlerDbContext context = new HemnetCrawlerDbContext();

            List<FinalBid> finalBids = context.FinalBids.OrderBy(fb => fb.SoldDate).ToList();

            foreach (Listing listing in context.Listings)
            {
                FinalBid match = finalBids.FirstOrDefault(fb => IsFinalBidAMatch(listing, fb));

                if (match != null)
                {
                    listing.FinalBidID = match.Id;
                    context.Update(listing);
                }
            }
            context.SaveChanges();
            context.Dispose();
        }

        static bool IsFinalBidAMatch(Listing listing, FinalBid finalBid)
        {
            return (listing.Published < finalBid.SoldDate &&
                    listing.HomeType == finalBid.HomeType &&
                    listing.PostalCode == finalBid.PostalCode &&
                    listing.Street == finalBid.Street);
        }
    }
}
