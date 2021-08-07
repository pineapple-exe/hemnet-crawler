using HemnetCrawler.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
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
            IListingRatingRepository listingRatingRepository = new ListingRatingRepository(context);

            HemnetCrawlerInteractor domain = new HemnetCrawlerInteractor(listingRepository, finalBidRepository, listingRatingRepository);

            ConsoleLogger logger = new ConsoleLogger();

            //SearchGatherListings(listingRepository, logger);
            SearchGatherFinalBids(finalBidRepository, logger);

            //domain.AddFinalBidToListing();

            context.Dispose();
        }

        static void GeneralSearchAndGather<T>(Action<IWebDriver> orderSearchResults, Action<IWebDriver, ILogger> addAgeFilter, Action<IWebDriver, T, ILogger> leafThroughPagesAndCreateRecords, T repository, ILogger logger)
        {
            using ChromeDriver driver = new ChromeDriver();
            
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
