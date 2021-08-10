using HemnetCrawler.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using HemnetCrawler.Domain.Repositories;
using HemnetCrawler.Data.Repositories;
using HemnetCrawler.Domain;
using HemnetCrawler.Domain.Interactors;

namespace HemnetCrawler.ConsoleApp.PageInteractives
{
    class Program
    {
        static void Main(string[] args)
        {
            HemnetCrawlerDbContext context = new HemnetCrawlerDbContext();

            IListingRepository listingRepository = new ListingRepository(context);
            IFinalBidRepository finalBidRepository = new FinalBidRepository(context);

            //FetchFinalBids fetchFinalBids = new FetchFinalBids(finalBidRepository);
            //FetchListings fetchListings = new FetchListings(listingRepository);

            FinalBidListingAssociater finalBidListingAssociater = new(listingRepository, finalBidRepository);

            ConsoleLogger logger = new();

            SearchGatherListings(listingRepository, logger);
            SearchGatherFinalBids(finalBidRepository, logger);

            finalBidListingAssociater.AddFinalBidToListing();

            context.Dispose();
        }

        static void GeneralSearchAndGather<T>(Action<IWebDriver> orderSearchResults, Action<IWebDriver, ILogger> addAgeFilter, Action<IWebDriver, T, ILogger> leafThroughPagesAndCreateRecords, T repository, ILogger logger)
        {
            using ChromeDriver driver = new();
            
            StartPage.EnterHemnet(driver);
            StartPage.AddSearchBase(driver, logger);
            orderSearchResults(driver);
            addAgeFilter(driver, logger);
            leafThroughPagesAndCreateRecords(driver, repository, logger);

            driver.Quit();
            driver.Dispose();
        }

        static void SearchGatherListings(IListingRepository repository, ILogger logger)
        {
            GeneralSearchAndGather(ListingsSearchResults.SortSearchResults, ListingsSearchResults.AddAgeFilter, Mixed.LeafThroughListingPagesAndCreateRecords, repository, logger);
        }

        static void SearchGatherFinalBids(IFinalBidRepository repository, ILogger logger)
        {
            GeneralSearchAndGather(FinalBidsSearchResults.SpecifyAndSortResults, FinalBidsSearchResults.AddAgeFilter, Mixed.LeafThroughFinalBidPagesAndCreateRecords, repository, logger);
        }
    }
}
