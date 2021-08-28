using OpenQA.Selenium;
using System;
using System.Linq;
using HemnetCrawler.Domain;
using System.Collections.Generic;
using HemnetCrawler.Domain.Repositories;
using HemnetCrawler.Domain.Entities;

namespace HemnetCrawler.ConsoleApp.PageInteractives
{
    internal class FinalBidsSearchResults
    {
        public static void CollectFinalBidLinks(IWebDriver driver, IFinalBidRepository repository, ILogger logger)
        {
            IReadOnlyCollection<IWebElement> searchResults = DriverBehavior.FindElements(driver, By.CssSelector("a.hcl-card"));
            List<string> links = searchResults.Select(e => e.GetAttribute("href")).ToList();
            int linksRemoved = links.RemoveAll(link => repository.GetAll().Any(f => f.HemnetId == int.Parse(link.Substring(link.LastIndexOf("-") + 1))));

            if (linksRemoved > 0)
                logger.Log($"{linksRemoved} Final Bids were skipped because they already exist in the database.");

            foreach (string link in links)
            {
                driver.Url = link;
                driver.Navigate();

                FinalBidPage.CreateFinalBidRecord(driver, repository, int.Parse(link.Substring(link.LastIndexOf("-") + 1)), logger);
            }
        }

        public static void SpecifyAndSortResults(IWebDriver driver)
        {
            DriverBehavior.FindElements(driver, By.CssSelector("div.result-type-toggle__label")).Where(e => e.Text == "Slutpriser").First().Click();

            IWebElement sortBy = DriverBehavior.FindElement(driver, By.CssSelector("#search-results-sort-by"));
            IReadOnlyCollection<IWebElement> options = DriverBehavior.FindElements(sortBy, By.CssSelector("option"));

            sortBy.Click();
            options.Where(e => e.Text == "Tidigast såld/överlåten först").First().Click();
        }

        public static void AddAgeFilter(IWebDriver driver, IFinalBidRepository repository, ILogger logger)
        {
            DateTimeOffset searchFrom = DateTimeOffset.Now;

            if (repository.GetAll().Any())
            {
                DateTimeOffset latestPublish = repository.GetAll().Select(finalBid => finalBid.SoldDate).Max();
                int daysDiff = (int)Math.Ceiling(Utils.GetTotalDays(latestPublish, DateTimeOffset.Now));

                string ageSearchFilter;

                if (daysDiff <= 84)
                {
                    ageSearchFilter = "search_sold_age_3m";
                    searchFrom = searchFrom.AddMonths(-3);
                }
                else if (daysDiff <= 168)
                {
                    ageSearchFilter = "search_sold_age_6m";
                    searchFrom = searchFrom.AddMonths(-6);
                }
                else if (daysDiff <= 336)
                {
                    ageSearchFilter = "search_sold_age_12m";
                    searchFrom = searchFrom.AddMonths(-12);
                }
                else
                {
                    ageSearchFilter = "search_sold_age_all";
                    searchFrom = DateTimeOffset.MinValue;
                }

                DriverBehavior.FindElements(driver, By.CssSelector("label.radio-token-list__label"))
                    .Where(e => e.GetAttribute("for") == $"{ageSearchFilter}").First().Click();

                DriverBehavior.FindElement(driver, By.CssSelector("button.search-form__submit-button")).Click();
            }

            logger.Log($"Final bids search initiated, from {searchFrom} and onward.");
        }

        public static void CollectHrefsForPreExistingFinalBids(IWebDriver driver, IFinalBidRepository repository, ILogger logger)
        {
            IReadOnlyCollection<IWebElement> searchResults = DriverBehavior.FindElements(driver, By.CssSelector("a.hcl-card"));
            List<string> hrefs = searchResults.Select(sr => sr.GetAttribute("href")).ToList();

            foreach (string href in hrefs)
            {
                int hemnetId = int.Parse(href.Substring(href.LastIndexOf("-") + 1));
                FinalBid finalBid = repository.GetAll().FirstOrDefault(fb => fb.HemnetId == hemnetId && string.IsNullOrEmpty(fb.Href));

                if (finalBid != null)
                {
                    finalBid.Href = href;
                    repository.UpdateFinalBid(finalBid);

                    logger.Log($"FinalBid with Id {finalBid.Id} was complemented with an href.");
                }
                else
                {
                    if (!repository.GetAll().Any(l => l.HemnetId == hemnetId))
                        logger.Log($"FinalBid with HemnetId {hemnetId} does not exist in database.");
                    else if (!(repository.GetAll().First(l => l.HemnetId == hemnetId).Href != null))
                        logger.Log($"FinalBid with Id {repository.GetAll().First(l => l.HemnetId == hemnetId).Id} already has href.");
                }
            }
        }
    }
}
