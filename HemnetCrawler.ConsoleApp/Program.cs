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
        private static void Main(string[] args)
        {
            using HemnetCrawlerDbContext context = new();

            IListingRepository listingRepository = new ListingRepository(context);
            IFinalBidRepository finalBidRepository = new FinalBidRepository(context);

            FinalBidListingAssociater finalBidListingAssociater = new(listingRepository, finalBidRepository);

            ConsoleLogger logger = new();

            SearchAndCollectListings(listingRepository, logger);
            SearchAndCollectFinalBids(finalBidRepository, logger);

            CollectHrefsForOldListings(listingRepository, logger);
            CollectHrefsForOldFinalBids(finalBidRepository, logger);

            CrawlerAddFinalBidsToListings(listingRepository, finalBidRepository, logger);
            finalBidListingAssociater.AlgorithmAddFinalBidsToListings(); //Dessa matchningar loggas inte, vilket skapar asymmetri.
        }

        private static void GeneralSearchAndCollect<T>(Action<IWebDriver> orderSearchResults, Action<IWebDriver, T, ILogger> addAgeFilter, Action<IWebDriver, T, ILogger> leafThroughPagesAndCreateRecords, T repository, ILogger logger)
        {
            using ChromeDriver driver = new();
            
            StartPage.EnterHemnet(driver);
            StartPage.AddSearchBase(driver, logger);

            orderSearchResults(driver);

            addAgeFilter(driver, repository, logger);
            leafThroughPagesAndCreateRecords(driver, repository, logger);

            driver.Quit();
        }

        private static void SearchAndCollectListings(IListingRepository repository, ILogger logger)
        {
            GeneralSearchAndCollect(ListingsSearchResults.SortSearchResults, ListingsSearchResults.AddAgeFilter, Mixed.LeafThroughListingPagesAndCreateRecords, repository, logger);
        }

        private static void SearchAndCollectFinalBids(IFinalBidRepository repository, ILogger logger)
        {
            GeneralSearchAndCollect(FinalBidsSearchResults.SpecifyAndSortResults, FinalBidsSearchResults.AddAgeFilter, Mixed.LeafThroughFinalBidPagesAndCreateRecords, repository, logger);
        }

        private static void GeneralComplementOldRecordsWithHrefs<T>(Action<IWebDriver> orderSearchResults, Action<IWebDriver, T, ILogger> collectHrefs, T repository, ILogger logger)
        {
            using ChromeDriver driver = new();

            StartPage.EnterHemnet(driver);
            StartPage.AddSearchBase(driver, logger);

            orderSearchResults(driver);
            collectHrefs(driver, repository, logger);

            driver.Quit();
        }

        private static void CollectHrefsForOldListings(IListingRepository repository, ILogger logger)
        {
            GeneralComplementOldRecordsWithHrefs(ListingsSearchResults.SortSearchResults, Mixed.AllPagesCollectHrefsForPreExistingListings, repository, logger);
        }

        private static void CollectHrefsForOldFinalBids(IFinalBidRepository repository, ILogger logger)
        {
            GeneralComplementOldRecordsWithHrefs(FinalBidsSearchResults.SpecifyAndSortResults, Mixed.AllPagesCollectHrefsForPreExistingFinalBids, repository, logger);
        }

        private static void CrawlerAddFinalBidsToListings(IListingRepository listingRepository, IFinalBidRepository finalBidRepository, ILogger logger)
        {
            using ChromeDriver driver = new();

            Mixed.CheckIfOldListingsAreSoldIfSoAddLinkedFinalBid(driver, finalBidRepository, listingRepository, logger);

            driver.Quit();
        }
    }
}
