﻿using HemnetCrawler.Data;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace HemnetCrawler.ConsoleApp
{
    public class ListingsSearchResults
    {
        public static void SortSearchResults(IWebDriver driver)
        {
            IWebElement sortBy = driver.FindElement(By.CssSelector("select.form-control__select"));
            sortBy.Click();
            ReadOnlyCollection<IWebElement> sortOptions = sortBy.FindElements(By.CssSelector("option"));
            sortOptions.Where(o => o.Text == "Äldst först").First().Click();
        }

        public static void AddAgeFilter(IWebDriver driver)
        {
            driver.FindElement(By.CssSelector("button.js-search-form-expand-more-filters")).Click();
            Thread.Sleep(3000);

            HemnetCrawlerDbContext context = new HemnetCrawlerDbContext();

            DateTimeOffset latestPublish = context.Listings.Select(listing => listing.Published).Max();
            int daysDiff = (int)Math.Ceiling(Utils.GetTotalDays(latestPublish, DateTimeOffset.Now));

            string ageSearchFilter;

            if (daysDiff <= 1)
            {
                ageSearchFilter = "search_age_1d";
            }
            else if (daysDiff <= 3)
            {
                ageSearchFilter = "search_age_3d";
            }
            else if (daysDiff <= 7)
            {
                ageSearchFilter = "search_age_1w";
            }
            else if (daysDiff <= 14)
            {
                ageSearchFilter = "search_age_2w";
            }
            else if (daysDiff <= 28)
            {
                ageSearchFilter = "search_age_1m";
            }
            else
            {
                ageSearchFilter = "search_age_all";
            }

            driver.FindElements(By.CssSelector("label.radio-token-list__label")).Where(e => e.GetAttribute("for") == $"{ageSearchFilter}").First().Click();
            Thread.Sleep(3000);

            driver.FindElement(By.CssSelector("button.search-form__submit-button")).Click();
            Thread.Sleep(3000);
        }

        public static List<ListingLink> CollectListingLinks(IWebDriver driver)
        {
            List<IWebElement> searchResults = driver.FindElements(By.CssSelector("a.js-listing-card-link")).ToList();
            Thread.Sleep(5000);
            searchResults.RemoveAll(l => Mixed.ElementContainsSpecificText(l, ".listing-card__label--type", "Nybyggnadsprojekt"));

            List<ListingLink> links = new List<ListingLink>();

            foreach (IWebElement searchResult in searchResults)
            {
                int listingLinkId = int.Parse(searchResult.FindElement(By.CssSelector("button.listing-card__save-button")).GetAttribute("data-listing-id"));

                HemnetCrawlerDbContext context = new HemnetCrawlerDbContext();
                if (context.Listings.Any(l => l.HemnetId == listingLinkId))
                {
                    continue;
                }

                bool newConstruction = Mixed.ElementContainsSpecificText(searchResult, ".listing-card__label--type", "Nyproduktion");

                links.Add(new ListingLink(listingLinkId, searchResult.GetAttribute("href"), newConstruction));
            }
            return links;
        }
    }
}