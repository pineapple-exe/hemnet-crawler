using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace HemnetCrawler.ConsoleApp.PageInteractives
{
    class Program
    { 
        static void Main(string[] args)
        {
            //GeneralSearchAndGather(FinalBidsSearchResults.SpecifyAndSortResults, FinalBidsSearchResults.AddAgeFilter, Mixed.LeafThroughFinalBidPagesAndCreateRecords); Fungerar!
            GeneralSearchAndGather(ListingsSearchResults.SortSearchResults, ListingsSearchResults.AddAgeFilter, Mixed.LeafThroughListingPagesAndCreateRecords);
        }

        static void GeneralSearchAndGather(Action<IWebDriver> orderSearchResults, Action<IWebDriver> addAgeFilter, Action<IWebDriver> leafThroughPagesAndCreateRecords)
        {
            ChromeDriver driver = new ChromeDriver();
            StartPage.EnterHemnet(driver);
            StartPage.AddSearchBase(driver);
            orderSearchResults(driver);
            addAgeFilter(driver);
            leafThroughPagesAndCreateRecords(driver);
        }
    }
}
