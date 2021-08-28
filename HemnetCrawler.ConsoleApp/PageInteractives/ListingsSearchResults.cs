using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HemnetCrawler.Domain;
using HemnetCrawler.Domain.Repositories;
using HemnetCrawler.Domain.Entities;

namespace HemnetCrawler.ConsoleApp.PageInteractives
{
    internal class ListingsSearchResults
    {
        public static void SortSearchResults(IWebDriver driver)
        {
            IWebElement sortBy = DriverBehavior.FindElement(driver, By.CssSelector("select.form-control__select"));
            sortBy.Click();
            IReadOnlyCollection<IWebElement> sortOptions = DriverBehavior.FindElements(sortBy, By.CssSelector("option"));
            sortOptions.Where(o => o.Text == "Äldst först").First().Click();
        }

        public static void AddAgeFilter(IWebDriver driver, IListingRepository repository, ILogger logger)
        {
            DriverBehavior.FindElement(driver, By.CssSelector("button.js-search-form-expand-more-filters")).Click();

            DateTimeOffset searchFrom = DateTimeOffset.Now;

            if (repository.GetAllListings().Any())
            {
                DateTimeOffset latestPublish = repository.GetAllListings().Select(listing => listing.Published).Max();
                int daysDiff = (int)Math.Ceiling(Utils.GetTotalDays(latestPublish, DateTimeOffset.Now));

                string ageSearchFilter;

                if (daysDiff <= 1)
                {
                    ageSearchFilter = "search_age_1d";
                    searchFrom = searchFrom.AddDays(-1);
                }
                else if (daysDiff <= 3)
                {
                    ageSearchFilter = "search_age_3d";
                    searchFrom = searchFrom.AddDays(-3);
                }
                else if (daysDiff <= 7)
                {
                    ageSearchFilter = "search_age_1w";
                    searchFrom = searchFrom.AddDays(-7);
                }
                else if (daysDiff <= 14)
                {
                    ageSearchFilter = "search_age_2w";
                    searchFrom = searchFrom.AddDays(-14);
                }
                else if (daysDiff <= 28)
                {
                    ageSearchFilter = "search_age_1m";
                    searchFrom = searchFrom.AddMonths(-1);
                }
                else
                {
                    ageSearchFilter = "search_age_all";
                    searchFrom = DateTimeOffset.MinValue;
                }

                DriverBehavior.FindElements(driver, By.CssSelector("label.radio-token-list__label"))
                    .Where(e => e.GetAttribute("for") == $"{ageSearchFilter}").First().Click();

                DriverBehavior.FindElement(driver, By.CssSelector("button.search-form__submit-button")).Click();
            }

            logger.Log($"Listing search initiated, from {searchFrom} and onward.");
        }

        private static bool ElementContainsSpecificText(IWebElement element, string selector, string content)
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

        public static List<ListingLink> CollectListingLinks(IWebDriver driver, IListingRepository repository, ILogger logger)
        {
            List<IWebElement> searchResults = DriverBehavior.FindElements(driver, By.CssSelector("li.normal-results__hit")).ToList();
            searchResults.RemoveAll(l => ElementContainsSpecificText(l, ".listing-card__label--type", "Nybyggnadsprojekt"));

            List<ListingLink> links = new();

            foreach (IWebElement searchResult in searchResults)
            {
                Regex premiumIdPattern = new("(?<={\"id\":\")\\d+");
                string dataContainer = searchResult.GetAttribute("data-gtm-item-info");
                int listingLinkId = int.Parse(premiumIdPattern.Match(dataContainer).Value);

                if (repository.GetAllListings().Any(l => l.HemnetId == listingLinkId))
                {
                    logger.Log($"Listing with HemnetId {listingLinkId} was skipped because it already existed in the database.");
                    continue;
                }

                bool newConstruction = ElementContainsSpecificText(searchResult, ".listing-card__label--type", "Nyproduktion");
                string href = DriverBehavior.FindElement(searchResult, By.CssSelector(".js-listing-card-link")).GetAttribute("href");

                links.Add(new ListingLink(listingLinkId, href, newConstruction));
            }
            return links;
        }

        public static void CollectHrefsForPreExistingListings(IWebDriver driver, IListingRepository repository, ILogger logger)
        {
            List<IWebElement> searchResults = DriverBehavior.FindElements(driver, By.CssSelector("li.normal-results__hit")).ToList();
            searchResults.RemoveAll(l => ElementContainsSpecificText(l, ".listing-card__label--type", "Nybyggnadsprojekt"));

            foreach (IWebElement searchResult in searchResults)
            {
                Regex premiumIdPattern = new("(?<={\"id\":\")\\d+");
                string dataContainer = searchResult.GetAttribute("data-gtm-item-info");
                int hemnetId = int.Parse(premiumIdPattern.Match(dataContainer).Value);

                Listing listing = repository.GetAllListings().FirstOrDefault(l => l.HemnetId == hemnetId && string.IsNullOrEmpty(l.Href));

                if (listing != null)
                {
                    listing.Href = DriverBehavior.FindElement(searchResult, By.CssSelector(".js-listing-card-link")).GetAttribute("href");
                    repository.UpdateListing(listing);

                    logger.Log($"Pre-existing Listing with Id {listing.Id} was complemented with href.");
                }
                else
                {
                    if (!repository.GetAllListings().Any(l => l.HemnetId == hemnetId))
                        logger.Log($"Listing with HemnetId {hemnetId} does not exist in database.");
                    else if (!(repository.GetAllListings().First(l => l.HemnetId == hemnetId).Href != null))
                        logger.Log($"Listing with Id {repository.GetAllListings().First(l => l.HemnetId == hemnetId).Id} already has href.");
                }
            }
        }
    }
}
