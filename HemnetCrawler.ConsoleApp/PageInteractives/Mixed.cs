using HemnetCrawler.Domain;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HemnetCrawler.ConsoleApp.PageInteractives
{
    internal static class Mixed
    {
        private static void DoThisThenNextPageLoop<T>(IWebDriver driver, T repository, ILogger logger, Action<IWebDriver, T, ILogger> doThis, bool condition = true)
        {
            string latestPage = driver.Url;

            while (condition)
            {
                doThis(driver, repository, logger);

                driver.Url = latestPage;
                IWebElement nextPageElement = DriverBehavior.FindElement(driver, By.CssSelector("a.next_page"));

                if (nextPageElement != null)
                {
                    driver.Url = nextPageElement.GetAttribute("href");
                    latestPage = driver.Url;
                }
                else return;
            }
        }

        public static void AllPagesCollectHrefsForPreExistingListings(IWebDriver driver, IListingRepository repository, ILogger logger)
        {
            bool hrefMissing = repository.GetAllListings().Any(l => l.Href == null);
            DoThisThenNextPageLoop(driver, repository, logger, ListingsSearchResults.CollectHrefsForPreExistingListings, hrefMissing);
        }

        public static void AllPagesCollectHrefsForPreExistingFinalBids(IWebDriver driver, IFinalBidRepository repository, ILogger logger)
        {
            bool hrefMissing = repository.GetAll().Any(fb => fb.Href == null);
            DoThisThenNextPageLoop(driver, repository, logger, FinalBidsSearchResults.CollectHrefsForPreExistingFinalBids, hrefMissing);
        }

        public static void LeafThroughListingPagesAndCreateRecords(IWebDriver driver, IListingRepository repository, ILogger logger)
        {
            DoThisThenNextPageLoop(driver, repository, logger, CollectListingLinksAndCreateRecords);
        }

        public static void LeafThroughFinalBidPagesAndCreateRecords(IWebDriver driver, IFinalBidRepository repository, ILogger logger)
        {
            DoThisThenNextPageLoop(driver, repository, logger, CollectFinalBidLinksAndCreateRecords);
        }

        private static void CollectListingLinksAndCreateRecords(IWebDriver driver, IListingRepository repository, ILogger logger)
        {
            List<ListingLink> listingLinks = ListingsSearchResults.CollectListingLinks(driver, repository, logger);

            foreach (ListingLink listingLink in listingLinks)
            {
                driver.Url = listingLink.Href;
                driver.Navigate();

                Listing listing = ListingPage.CreateListingEntity(driver, listingLink);

                if (listing != null)
                {
                    List<Image> images = ListingPage.CreateImageEntities(driver, listing).ToList();

                    repository.AddListing(listing);

                    foreach (Image img in images)
                    {
                        repository.AddImage(img);
                    }

                    logger.Log($"A new Listing with Id {listing.Id} was created. Located on {listing.Street}, {listing.City}.");
                }
            }
        }

        private static void CollectFinalBidLinksAndCreateRecords(IWebDriver driver, IFinalBidRepository repository, ILogger logger)
        {
            IReadOnlyCollection<IWebElement> searchResults = DriverBehavior.FindElements(driver, By.CssSelector("a.hcl-card"));
            List<string> links = searchResults.Select(sr => sr.GetAttribute("href")).ToList();
            int linksRemoved = links.RemoveAll(l => repository.GetAll().Any(f => f.HemnetId == int.Parse(l.Substring(l.LastIndexOf("-") + 1))));

            if (linksRemoved > 0)
                logger.Log($"{linksRemoved} Final Bids were skipped because they already exist in database.");

            foreach (string link in links)
            {
                driver.Url = link;
                driver.Navigate();

                FinalBidPage.CreateFinalBidRecord(driver, repository, int.Parse(link.Substring(link.LastIndexOf("-") + 1)), logger);
            }
        }

        public static void CheckIfOldListingsAreSoldIfSoAddLinkedFinalBid(IWebDriver driver, IFinalBidRepository finalBidRepository, IListingRepository listingRepository, ILogger logger)
        {
            List<Listing> listings = listingRepository.GetAllListings().Where(l => l.FinalBidId == null && !string.IsNullOrEmpty(l.Href)).ToList();

            foreach (Listing listing in listings)
            {
                int correspondingFinalBidHemnetId;

                StartPage.EnterHemnet(driver, listing.Href);

                IWebElement removedListingButton = DriverBehavior.FindElement(driver, By.CssSelector("a.hcl-button.hcl-button--primary.hcl-button--full-width.qa-removed-listing-button"), true);
                
                if (removedListingButton == null) return;
                else if (removedListingButton.Text == "Visa slutpriset för bostaden")
                {
                    string finalBidHref = removedListingButton.GetAttribute("href");
                    correspondingFinalBidHemnetId = int.Parse(finalBidHref.Substring(finalBidHref.LastIndexOf("-") + 1));

                    if (finalBidRepository.GetAll().Any(fb => fb.HemnetId == correspondingFinalBidHemnetId))
                    {
                        listing.FinalBid = finalBidRepository.GetAll().Where(fb => fb.HemnetId == correspondingFinalBidHemnetId).First();
                        listingRepository.UpdateListing(listing);
                    }
                    else
                    {
                        driver.Url = finalBidHref;
                        driver.Navigate();

                        listing.FinalBid = FinalBidPage.CreateFinalBidRecord(driver, finalBidRepository, correspondingFinalBidHemnetId, logger);
                        listingRepository.UpdateListing(listing);
                    }
                    logger.Log($"FinalBid with Id {listing.FinalBidId} was perfectly matched with Listing {listing.Id}.");
                }
            }
        }
    }
}

