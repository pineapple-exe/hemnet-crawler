using HemnetCrawler.Data;
using HemnetCrawler.Data.Entities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace HemnetCrawler.ConsoleApp
{
    public class ListingPage
    {
        public static bool ContainsRepeatedValue(List<IWebElement> labels, out int index)
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

        public static void InterpretTable(Listing listing, Dictionary<string, string> labelsAndValues)
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
        public static void CreateListingEntity(IWebDriver driver, HemnetCrawlerDbContext context, ListingLink listingLink, Listing listing)
        {
            if (driver.PageSource.Contains("removed-listing"))
                return;

            listing.LastUpdated = DateTimeOffset.Now;
            listing.HemnetId = listingLink.Id;
            listing.NewConstruction = listingLink.NewConstruction;

            Regex publishedPattern = new Regex("(?<=\"publication_date\":\")\\d{4}-\\d{2}-\\d{2}");
            string publishedDate = publishedPattern.Match(driver.PageSource).Value;
            listing.Published = DateTimeOffset.Parse(publishedDate);

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

            context.Add(listing);
            context.SaveChanges();
        }

        public static void CreateImageEntities(IWebDriver driver, HemnetCrawlerDbContext context, Listing listing)
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
                context.SaveChanges();
            }
        }

        public static void CreateEntities(IWebDriver driver, List<ListingLink> listingLinks)
        {
            foreach (ListingLink listingLink in listingLinks)
            {
                driver.Url = listingLink.Href;
                driver.Navigate();
                Thread.Sleep(2000);

                HemnetCrawlerDbContext context = new HemnetCrawlerDbContext();
                Listing listing = new Listing();

                CreateListingEntity(driver, context, listingLink, listing);
                CreateImageEntities(driver, context, listing);
            }
        }
    }
}
