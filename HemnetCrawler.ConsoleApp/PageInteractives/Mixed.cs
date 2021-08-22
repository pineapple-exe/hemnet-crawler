using HemnetCrawler.Data;
using HemnetCrawler.Domain;
using HemnetCrawler.Domain.Repositories;
using OpenQA.Selenium;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

namespace HemnetCrawler.ConsoleApp.PageInteractives
{
    internal static class Mixed
    {
        public static bool ElementContainsSpecificText(IWebElement element, string selector, string content)
        {
            List<IWebElement> matches = new();
            matches.AddRange(element.FindElements(By.CssSelector(selector)));

            foreach (IWebElement match in matches)
            {
                if (match.Text == content)
                    return true;
            }
            return false;
        }

        public static void LeafThroughListingPagesAndCreateRecords(IWebDriver driver, IListingRepository repository, ILogger logger)
        {
            string latestPage = driver.Url;

            while (true)
            {
                ListingPage.CreateRecords(driver, repository, ListingsSearchResults.CollectListingLinks(driver, logger), logger);
                driver.Url = latestPage;
                latestPage = DriverBehavior.FindElement(driver, By.CssSelector("a.next_page")).GetAttribute("href");

                if (latestPage != null)
                {
                    driver.Url = latestPage;
                }
                else return;
            }
        }

        public static void LeafThroughFinalBidPagesAndCreateRecords(IWebDriver driver, IFinalBidRepository repository, ILogger logger)
        {
            string latestPage = driver.Url;
            using HemnetCrawlerDbContext context = new HemnetCrawlerDbContext();

            while (true)
            {
                Thread.Sleep(2000);
                IReadOnlyCollection<IWebElement> searchResults = DriverBehavior.FindElements(driver, By.CssSelector("a.hcl-card"));
                List<string> links = searchResults.Select(e => e.GetAttribute("href")).ToList();
                int linksRemoved = links.RemoveAll(link => context.FinalBids.Any(f => f.HemnetId == int.Parse(link.Substring(link.LastIndexOf("-") + 1))));

                if (linksRemoved > 0)
                    logger.Log($"{linksRemoved} Final Bids were skipped because they already exist in the database.");

                foreach (string link in links)
                {
                    driver.Url = link;
                    driver.Navigate();

                    FinalBidPage.CreateFinalBidRecord(driver, repository, int.Parse(link.Substring(link.LastIndexOf("-") + 1)), logger);
                }

                driver.Url = latestPage;
                latestPage = DriverBehavior.FindElement(driver, By.CssSelector("a.next_page")).GetAttribute("href");

                if (latestPage != null)
                {
                    driver.Url = latestPage;
                }
                else return;
            }
        }
    }
}

