using HemnetCrawler.Domain;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace HemnetCrawler.ConsoleApp.PageInteractives
{
    internal class ListingPage
    {
        private static bool ContainsRepeatedValue(List<IWebElement> labels, out int index)
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

        private static void InterpretTable(Listing listing, Dictionary<string, string> labelsAndValues)
        {
            foreach (KeyValuePair<string, string> pair in labelsAndValues)
            {
                switch (pair.Key)
                {
                    case "Pris/m²":
                        listing.PricePerSquareMeter = Utils.DigitPurist(pair.Value);
                        break;

                    case "Bostadstyp":
                        listing.HomeType = pair.Value;
                        break;

                    case "Upplåtelseform":
                        listing.OwnershipType = pair.Value;
                        break;

                    case "Antal rum":
                        listing.Rooms = pair.Value;
                        break;

                    case "Boarea":
                        listing.LivingArea = Utils.DigitPurist(pair.Value);
                        break;

                    case "Biarea":
                        listing.BiArea = Utils.DigitPurist(pair.Value);
                        break;

                    case "Tomtarea":
                        listing.PropertyArea = Utils.DigitPurist(pair.Value);
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
                        listing.Fee = Utils.DigitPurist(pair.Value);
                        break;

                    case "Driftkostnad":
                        listing.Utilities = Utils.DigitPurist(pair.Value);
                        break;

                    case "Energiklass":
                        listing.EnergyClassification = pair.Value;
                        break;

                    case "Antal besök":
                        listing.Visits = Utils.DigitPurist(pair.Value);
                        break;
                }
            }
        }

        private static Listing CreateListingEntity(IWebDriver driver, ListingLink listingLink)
        {
            if (driver.PageSource.Contains("removed-listing"))
                return null;

            Listing listing = new Listing();

            listing.LastUpdated = DateTimeOffset.Now;
            listing.HemnetId = listingLink.Id;
            listing.NewConstruction = listingLink.NewConstruction;

            Regex publishedPattern = new Regex("(?<=\"publication_date\":\")\\d{4}-\\d{2}-\\d{2}");
            string publishedDate = publishedPattern.Match(driver.PageSource).Value;
            listing.Published = DateTimeOffset.Parse(publishedDate);

            Regex postalCodePattern = new Regex("(?<=\"postalCode\":\\s)\\d{3}\\s?\\d{2}");
            string postalCode = postalCodePattern.Match(driver.PageSource).Value;
            if (postalCode != "")
               listing.PostalCode = Utils.DigitPurist(postalCode);

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

            InterpretTable(listing, labelsAndValues);

            return listing;
        }

        private static IEnumerable<Image> CreateImageEntities(IWebDriver driver, Listing listing)
        {
            bool hasImages = driver.PageSource.Contains("property-gallery__fullscreen-button");
            
            if (hasImages)
            { 
                IWebElement expandImagesButton = driver.FindElement(By.CssSelector("button.property-gallery__fullscreen-button"));
                expandImagesButton.Click();

                ReadOnlyCollection<IWebElement> imgElements = driver.FindElements(By.CssSelector("img.all-images__image all-images__image--loaded"));
                WebClient webWizard = new WebClient();

                foreach (IWebElement imageContainer in imgElements)
                {
                    Image image = new Image
                    {
                        Listing = listing,
                        Data = webWizard.DownloadData(new Uri(imageContainer.GetAttribute("src"))),
                        ContentType = "Unknown"
                    };

                    yield return image;
                }

                IWebElement closeImages = driver.FindElement(By.CssSelector("button.fullscreen-action-bar__close-button"));
                closeImages.Click();
            }
        }

        public static void CreateRecords(IWebDriver driver, IListingRepository repository, List<ListingLink> listingLinks, ILogger logger)
        {
            foreach (ListingLink listingLink in listingLinks)
            {
                driver.Url = listingLink.Href;
                driver.Navigate();
                Thread.Sleep(2000);

                Listing listing = CreateListingEntity(driver, listingLink);

                if (listing != null)
                {
                    repository.AddListing(listing);
                    logger.Log($"A new Listing with id {listing.Id} was created. Located on {listing.Street}, {listing.City}.");

                    IEnumerable<Image> images = CreateImageEntities(driver, listing);
                    foreach (Image img in images)
                    {
                        repository.AddImage(img);
                    }
                }
            }
        }
    }
}
