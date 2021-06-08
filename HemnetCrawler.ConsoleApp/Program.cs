using HemnetCrawler.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;
using HemnetCrawler.Data.Repositories;
using HemnetCrawler.Domain;

namespace HemnetCrawler.ConsoleApp.PageInteractives
{
    class Program
    {
        static void Main(string[] args)
        {
            HemnetCrawlerDbContext context = new HemnetCrawlerDbContext();

            IListingRepository listingRepository = new ListingRepository(context);
            IFinalBidRepository finalBidRepository = new FinalBidRepository(context);

            HemnetCrawlerDomain domain = new HemnetCrawlerDomain(listingRepository, finalBidRepository);

            SearchGatherListings(listingRepository);
            SearchGatherFinalBids(finalBidRepository);

            domain.AddFinalBidToListing();

            context.Dispose();
        }

        static void GeneralSearchAndGather<T>(Action<IWebDriver> orderSearchResults, Action<IWebDriver> addAgeFilter, Action<IWebDriver, T> leafThroughPagesAndCreateRecords, T repository)
        {
            ChromeDriver driver = new ChromeDriver();
            
            StartPage.EnterHemnet(driver);
            StartPage.AddSearchBase(driver);
            orderSearchResults(driver);
            addAgeFilter(driver);
            leafThroughPagesAndCreateRecords(driver, repository);

            driver.Quit();
            driver.Dispose();
        }

        static void SearchGatherListings(IListingRepository repository)
        {
            GeneralSearchAndGather(ListingsSearchResults.SortSearchResults, ListingsSearchResults.AddAgeFilter, Mixed.LeafThroughListingPagesAndCreateRecords, repository);
        }

        static void SearchGatherFinalBids(IFinalBidRepository repository)
        {
            GeneralSearchAndGather(FinalBidsSearchResults.SpecifyAndSortResults, FinalBidsSearchResults.AddAgeFilter, Mixed.LeafThroughFinalBidPagesAndCreateRecords, repository);
        }
    }
}
