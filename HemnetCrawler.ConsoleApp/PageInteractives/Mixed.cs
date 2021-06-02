using HemnetCrawler.Data;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace HemnetCrawler.ConsoleApp
{
    internal static class Mixed
    {
        public static bool ElementContainsSpecificText(IWebElement element, string selector, string content)
        {
            List<IWebElement> matches = new List<IWebElement>();
            matches.AddRange(element.FindElements(By.CssSelector(selector)));

            foreach (IWebElement match in matches)
            {
                if (match.Text == content)
                    return true;
            }
            return false;
        }

        public static void LeafThroughListingPagesAndCreateRecords(IWebDriver driver)
        {
            string latestPage = driver.Url;

            while (true)
            {
                ListingPage.CreateEntities(driver, ListingsSearchResults.CollectListingLinks(driver));
                driver.Url = latestPage;
                Thread.Sleep(2000);

                try
                {
                    latestPage = driver.FindElement(By.CssSelector("a.next_page")).GetAttribute("href");
                    driver.Url = latestPage;
                    Thread.Sleep(2000);
                }
                catch (NoSuchElementException)
                {
                    break;
                }
            }
            driver.Dispose();
        }

        public static void LeafThroughFinalBidPagesAndCreateRecords(IWebDriver driver)
        {
            string latestPage = driver.Url;
            HemnetCrawlerDbContext context = new HemnetCrawlerDbContext();

            while (true)
            {
                Thread.Sleep(2000);
                ReadOnlyCollection<IWebElement> searchResults = driver.FindElements(By.CssSelector("a.hcl-card"));
                List<string> links = searchResults.Select(e => e.GetAttribute("href")).ToList();
                links.RemoveAll(link => context.FinalBids.Any(f => f.HemnetId == int.Parse(link.Substring(link.LastIndexOf("-") + 1))));

                foreach (string link in links)
                {
                    driver.Url = link;
                    driver.Navigate();
                    Thread.Sleep(2000);

                    FinalBidPage.CreateFinalBidRecords(driver, int.Parse(link.Substring(link.LastIndexOf("-") + 1)), context);
                }

                driver.Url = latestPage;
                Thread.Sleep(2000);

                try
                {
                    latestPage = driver.FindElement(By.CssSelector("a.next_page")).GetAttribute("href");
                    driver.Url = latestPage;
                    Thread.Sleep(2000);
                }
                catch (NoSuchElementException)
                {
                    break;
                }
            }
            driver.Dispose();
        }
    }
}

