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

namespace HemnetCrawler.ConsoleApp
{
    class Program
    { 
        static void Main(string[] args)
        {
            SearchAndGather();
        }

        static void SearchAndGather()
        {
            IWebDriver driver = new ChromeDriver
            {
                Url = "https://www.hemnet.se/"
            };
            driver.Navigate();
            Thread.Sleep(1000);

            IWebElement acceptCookiesButton = driver.FindElement(By.CssSelector("button.hcl-button--primary"));
            acceptCookiesButton.Click();
            Thread.Sleep(1000);

            IWebElement searchBox = driver.FindElement(By.CssSelector("#area-search-input-box"));
            searchBox.Click();
            Thread.Sleep(1000);

            searchBox.SendKeys("Västra");
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector(".item-first.item.alt")).Click();
            searchBox.Click();
            Thread.Sleep(1000);

            searchBox.SendKeys("Hallands");
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector(".item-first.item.alt")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector(".js-submit-button.js-show-on-forsale")).Click();
            Thread.Sleep(5000);

            string latestPage = driver.Url;
            while (true)
            {
                CreateEntities(driver, CollectListingLinks(driver));
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

        static bool ContainsSpecificText(IWebElement element, string selector, string content)
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

        static List<ListingLink> CollectListingLinks(IWebDriver driver)
        {
            List<IWebElement> searchResults = driver.FindElements(By.CssSelector("a.js-listing-card-link")).ToList();
            Thread.Sleep(5000);
            searchResults.RemoveAll(l => ContainsSpecificText(l, ".listing-card__label--type", "Nybyggnadsprojekt"));

            List<ListingLink> links = new List<ListingLink>();

            foreach (IWebElement searchResult in searchResults)
            {
                int listingLinkId = int.Parse(searchResult.FindElement(By.CssSelector("button.listing-card__save-button")).GetAttribute("data-listing-id"));

                HemnetCrawlerDbContext context = new HemnetCrawlerDbContext();
                if (context.Listings.Any(l => l.HemnetId == listingLinkId))
                {
                    continue;
                }

                bool newConstruction = ContainsSpecificText(searchResult, ".listing-card__label--type", "Nyproduktion");

                links.Add(new ListingLink(listingLinkId, searchResult.GetAttribute("href"), newConstruction));
            }
            return links;
        }

        static int DigitPurist(string impure)
        {
            Regex nonDigitPattern = new Regex("\\D+");
            return int.Parse(nonDigitPattern.Replace(impure, ""));
        }

        static void InterpretListingTable(Listing listing, Dictionary<string, string> labelsAndValues)
        {
            foreach (KeyValuePair<string, string> pair in labelsAndValues)
            {
                switch (pair.Key)
                {
                    case "Pris/m²":
                        listing.PricePerSquareMeter = DigitPurist(pair.Value);
                        break;

                    case "Bostadstyp":
                        listing.HomeType = pair.Value;
                        break;

                    case "Upplåtelseform":
                        listing.OwnershipType = pair.Value;
                        break;

                    case "Antal rum":
                        listing.Rooms = DigitPurist(pair.Value);
                        break;

                    case "Boarea":
                        listing.LivingArea = DigitPurist(pair.Value);
                        break;

                    case "Biarea":
                        listing.BiArea = DigitPurist(pair.Value);
                        break;

                    case "Tomtarea":
                        listing.PropertyArea = DigitPurist(pair.Value);
                        break;

                    case "Balkong":
                        listing.Balcony = pair.Value == "Ja";
                        break;

                    case "Våning":
                        listing.Floor = pair.Value;
                        break;

                    case "Byggår":
                        listing.ConstructionYear = pair.Value;
                        break;

                    case "Förening":
                        listing.HomeOwnersAssociation = pair.Value;
                        break;

                    case "Avgift":
                        listing.Fee = DigitPurist(pair.Value);
                        break;

                    case "Driftkostnad":
                        listing.Utilities = DigitPurist(pair.Value);
                        break;

                    case "Energiklass":
                        listing.EnergyClassification = pair.Value;
                        break;

                    case "Antal besök":
                        listing.Visits = DigitPurist(pair.Value);
                        break;

                    case "Dagar på Hemnet":
                        listing.DaysOnHemnet = DigitPurist(pair.Value);
                        break;
                }
            }
        }

        static bool ContainsRepeatedValue(List<IWebElement> labels, out int index)
        {
            if (labels.Any(l => l.Text == "Förening"))
            {
                index = labels.IndexOf(labels.First(l => l.Text == "Förening"));
                return true;
            }
            else
            {
                index = -1;
                return false;
            }
        }

        static void CreateListingEntity(IWebDriver driver, ListingLink listingLink, Listing listing)
        {
            listing.HemnetId = listingLink.Id;
            listing.NewConstruction = listingLink.NewConstruction;

            listing.Street = driver.FindElement(By.CssSelector("h1.qa-property-heading.hcl-heading.hcl-heading--size2")).Text;
            Thread.Sleep(1000);

            listing.City = driver.FindElement(By.CssSelector("span.property-address__area")).Text;
            Thread.Sleep(1000);

            string price = driver.FindElement(By.CssSelector(".property-info__price.qa-property-price")).Text;
            listing.Price = int.TryParse(price[0..^3].Replace(" ", ""), out int integerPrice) ? integerPrice : (int?)null;
            Thread.Sleep(1000);

            listing.Description = driver.FindElement(By.CssSelector(".property-description")).Text;
            Thread.Sleep(1000);

            var attributeLabels = new List<IWebElement>();
            var attributeValues = new List<IWebElement>();

            attributeLabels.AddRange(driver.FindElements(By.CssSelector(".property-attributes-table__label")));
            attributeLabels.AddRange(driver.FindElements(By.CssSelector(".property-visits-counter__row-label")));

            attributeValues.AddRange(driver.FindElements(By.CssSelector(".property-attributes-table__value")));
            attributeValues.AddRange(driver.FindElements(By.CssSelector(".property-visits-counter__row-value")));
            if (ContainsRepeatedValue(attributeLabels, out int index))
                attributeValues.Remove(attributeValues[index]);

            var labelsAndValues = new Dictionary<string, string>();
            for (int i = 0; i < attributeLabels.Count; i++)
            {
                if (!String.IsNullOrEmpty(attributeLabels[i].Text))
                    labelsAndValues.Add(attributeLabels[i].Text, attributeValues[i].Text);
            }

            InterpretListingTable(listing, labelsAndValues);
        }

        static void CreateImageEntities(IWebDriver driver, HemnetCrawlerDbContext context, Listing listing)
        {
            ReadOnlyCollection<IWebElement> imageContainers = driver.FindElements(By.CssSelector(".gallery-carousel__image-touchable img"));
            WebClient webWizard = new WebClient();
            foreach (IWebElement imageContainer in imageContainers)
            {
                Image image = new Image
                {
                    Listing = listing
                };
                image.Data = webWizard.DownloadData(new Uri(imageContainer.GetAttribute("src")));
                image.ContentType = "Unknown";
                context.Add(image);
            }
        }

        static void CreateEntities(IWebDriver driver, List<ListingLink> listingLinks)
        {
            foreach (ListingLink listingLink in listingLinks)
            {
                driver.Url = listingLink.Href;
                driver.Navigate();
                Thread.Sleep(2000);

                HemnetCrawlerDbContext context = new HemnetCrawlerDbContext();
                Listing listing = new Listing();

                CreateListingEntity(driver, listingLink, listing);
                context.Add(listing);

                CreateImageEntities(driver, context, listing);

                context.SaveChanges();
            }
        }
    }
}
