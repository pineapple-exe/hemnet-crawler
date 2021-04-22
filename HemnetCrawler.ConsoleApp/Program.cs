using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using HemnetCrawler.Data;
using HemnetCrawler.Data.Entities;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Net;

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

        //static void SearchAndGatherFinalBids()
        //{
        //    ChromeDriver driver = new ChromeDriver
        //    {
        //        Url = "https://www.hemnet.se/"
        //    };
        //    driver.Navigate();
        //    driver.Manage().Window.Maximize();
        //    Thread.Sleep(1000);

        //    IWebElement acceptCookiesButton = driver.FindElement(By.CssSelector("button.hcl-button--primary"));
        //    acceptCookiesButton.Click();
        //    Thread.Sleep(1000);

        //    IWebElement searchBox = driver.FindElement(By.CssSelector("#area-search-input-box"));
        //    searchBox.Click();
        //    Thread.Sleep(1000);

        //    searchBox.SendKeys("Västra");
        //    Thread.Sleep(3000);
        //    driver.FindElement(By.CssSelector(".item-first.item.alt")).Click();
        //    searchBox.Click();
        //    Thread.Sleep(1000);

        //    searchBox.SendKeys("Hallands");
        //    Thread.Sleep(3000);
        //    driver.FindElement(By.CssSelector(".item-first.item.alt")).Click();
        //    Thread.Sleep(1000);

        //    driver.FindElement(By.CssSelector(".js-submit-button.js-show-on-forsale")).Click();
        //    Thread.Sleep(7000);

        //    driver.FindElements(By.CssSelector("div.result-type-toggle__label")).Where(e => e.Text == "Slutpriser").First().Click();
        //    Thread.Sleep(2000);

        //    var sortBy = driver.FindElement(By.CssSelector("#search-results-sort-by"));
        //    var options = sortBy.FindElements(By.CssSelector("option"));

        //    sortBy.Click();
        //    options.Where(e => e.Text == "Tidigast såld/överlåten först").First().Click();

        //    FinalBidsSearchResults.AddAgeFilter(driver);

        //string latestPage = driver.Url;
        //List<FinalBid> finalBids = new List<FinalBid>();
        //HemnetCrawlerDbContext context = new HemnetCrawlerDbContext();

        //while (true)
        //{
        //    Thread.Sleep(2000);
        //    ReadOnlyCollection<IWebElement> searchResults = driver.FindElements(By.CssSelector("a.hcl-card"));
        //    List<string> links = searchResults.Select(e => e.GetAttribute("href")).ToList();
        //    links.RemoveAll(link => context.FinalBids.Any(f => f.HemnetId == int.Parse(link.Substring(link.LastIndexOf("-") + 1))));

        //    foreach (string link in links)
        //    {
        //        driver.Url = link;
        //        Thread.Sleep(2000);

        //        FinalBid finalBid = new FinalBid();

        //        finalBid.HemnetId = int.Parse(link.Substring(link.LastIndexOf("-") + 1));
        //        finalBid.LastUpdated = DateTimeOffset.Now;
        //        finalBid.Street = driver.FindElement(By.CssSelector("h1.hcl-heading")).Text.Replace("Slutpris", "").Trim();
        //        finalBid.Price = Utils.DigitPurist(driver.FindElement(By.CssSelector("span.sold-property__price-value")).Text);
        //        finalBid.SoldDate = DateTimeOffset.Parse(driver.FindElement(By.CssSelector("time")).GetAttribute("datetime"));

        //        Regex redundantWhitespace = new Regex("\\s{2,}");
        //        string city = driver.FindElement(By.CssSelector("p.sold-property__metadata.qa-sold-property-metadata")).Text.Replace("\n", "");
        //        city = redundantWhitespace.Replace(city, " ");
        //        finalBid.City = city.Split("-")[1].Trim();

        //        var labels = driver.FindElements(By.CssSelector("dt.sold-property__attribute")).Select(e => e.Text).ToList();
        //        var values = driver.FindElements(By.CssSelector("dd.sold-property__attribute-value")).Select(e => e.Text).ToList();

        //        var labelsAndValues = new Dictionary<string, string>();
        //        for (int i = 0; i < labels.Count; i++)
        //        {
        //            if (!String.IsNullOrEmpty(labels[i]))
        //                labelsAndValues.Add(labels[i], values[i]);
        //        }

        //        FinalBidPage.InterpretTable(finalBid, labelsAndValues);

        //        finalBids.Add(finalBid);
        //    }

        //    foreach (FinalBid finalBid in finalBids)
        //    {
        //        context.Add(finalBid);
        //    }
        //    context.SaveChanges();
        //    finalBids.Clear();

        //    driver.Url = latestPage;
        //    Thread.Sleep(2000);

        //    try
        //    {
        //        latestPage = driver.FindElement(By.CssSelector("a.next_page")).GetAttribute("href");
        //        driver.Url = latestPage;
        //        Thread.Sleep(2000);
        //    }
        //    catch (NoSuchElementException)
        //    {
        //        break;
        //    }
        //}

        //driver.Dispose();
        //}
    }
}
