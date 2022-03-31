using HemnetCrawler.Domain.Entities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
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
                        listing.PricePerSquareMeter = Utils.NumberPurist(pair.Value);
                        break;

                    case "Bostadstyp":
                        listing.HomeType = pair.Value;
                        break;

                    case "Upplåtelseform":
                        listing.OwnershipType = pair.Value;
                        break;

                    case "Antal rum":
                        listing.Rooms = Utils.NumberPuristDouble(pair.Value);
                        break;

                    case "Boarea":
                        listing.LivingArea = Utils.NumberPurist(pair.Value);
                        break;

                    case "Biarea":
                        listing.BiArea = Utils.NumberPurist(pair.Value);
                        break;

                    case "Tomtarea":
                        listing.PropertyArea = Utils.NumberPurist(pair.Value);
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
                        listing.Fee = Utils.NumberPurist(pair.Value);
                        break;

                    case "Driftkostnad":
                        listing.Utilities = Utils.NumberPurist(pair.Value);
                        break;

                    case "Energiklass":
                        listing.EnergyClassification = pair.Value;
                        break;

                    case "Antal besök":
                        listing.Visits = String.IsNullOrEmpty(pair.Value) ? 0 : Utils.NumberPurist(pair.Value);
                        break;
                }
            }
        }

        public static Listing CreateListingEntity(IWebDriver driver, ListingLink listingLink)
        {
            if (driver.PageSource.Contains("removed-listing")) return null;

            if (driver.PageSource.Contains("page-not-found-illustration")) driver.Navigate().Refresh();

            Listing listing = new();

            listing.LastUpdated = DateTimeOffset.Now;
            listing.HemnetId = listingLink.Id;
            listing.Href = listingLink.Href;
            listing.NewConstruction = listingLink.NewConstruction;

            Regex postalCodePattern = new("(?<=\"postalCode\":\\s)\\d{3}\\s?\\d{2}");
            string postalCode = postalCodePattern.Match(driver.PageSource).Value;
            if (postalCode != "")
               listing.PostalCode = Utils.NumberPurist(postalCode);

            Regex publishedPattern = new("(?<=\"publication_date\":\")\\d{4}-\\d{2}-\\d{2}");
            string publishedDate = publishedPattern.Match(driver.PageSource).Value;
            listing.Published = DateTime.Parse(publishedDate);

            listing.Description = DriverBehavior.FindElement(driver, By.CssSelector(".property-description")).Text;

            listing.Street = DriverBehavior.FindElement(driver, By.CssSelector("h1.qa-property-heading.hcl-heading.hcl-heading--size2")).Text;

            listing.City = DriverBehavior.FindElement(driver, By.CssSelector("span.property-address__area")).Text;

            string price = DriverBehavior.FindElement(driver, By.CssSelector(".qa-property-price")).Text;
            listing.Price = int.TryParse(price[0..^3].Replace(" ", ""), out int integerPrice) ? integerPrice : null;

            List<IWebElement> attributeLabels = new();
            List<IWebElement> attributeValues = new();

            attributeLabels.AddRange(DriverBehavior.FindElements(driver, By.CssSelector(".property-attributes-table__label")));
            attributeValues.AddRange(DriverBehavior.FindElements(driver, By.CssSelector(".property-attributes-table__value")));

            IWebElement visitsCounterLabel = DriverBehavior.FindElement(driver, By.CssSelector(".property-visits-counter__row-label"), true);
            if (visitsCounterLabel != null)
            {
                attributeLabels.Add(visitsCounterLabel);
                attributeValues.Add(DriverBehavior.FindElement(driver, By.CssSelector(".property-visits-counter__row-value")));
            }

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

        public static string DetectMediaType(byte[] imageData)
        {
            ushort jpegSignature = BitConverter.ToUInt16(imageData, 0);
            if (jpegSignature == 55551) 
                return "image/jpeg";

            ulong pngSignature = BitConverter.ToUInt64(imageData, 0);
            if (pngSignature == 727905341920923785UL) 
                return "image/png";

            return "";
        }

        public static IEnumerable<Image> CreateImageEntities(IWebDriver driver, Listing listing)
        {
            bool hasImages = driver.PageSource.Contains("property-gallery__fullscreen-button");
            
            if (hasImages)
            { 
                IWebElement expandImagesButton = DriverBehavior.FindElement(driver, By.CssSelector("button.property-gallery__fullscreen-button"));
                expandImagesButton.Click();

                var imgElementContainers = DriverBehavior.FindElements(driver, By.CssSelector("div.all-images__image-container"));
                WebClient webWizard = new();
                int yPosition = 0;

                foreach (IWebElement container in imgElementContainers)
                {
                    yPosition += container.Size.Height;
                    IWebElement imgElement;
                    Uri imgSrc;

                    DriverBehavior.Scroll(driver, "div.all-images", 0, yPosition);

                    int urlSearchAttempts = 0;

                    while (true)
                    {
                        urlSearchAttempts++;

                        imgElement = DriverBehavior.FindElement(container, By.CssSelector("img.all-images__image.all-images__image--loaded"));

                        try
                        { 
                            string potentiallyLegitUri = imgElement.GetAttribute("src");

                            if (potentiallyLegitUri.StartsWith("http://") || potentiallyLegitUri.StartsWith("https://"))
                            {
                                imgSrc = new Uri(potentiallyLegitUri);
                                break;
                            }
                            else if (urlSearchAttempts > 10)
                            {
                                throw new Exception("Never ending loop de loop");
                            }
                            else
                            {
                                Thread.Sleep(250);
                                continue;
                            }
                        }
                        catch (StaleElementReferenceException)
                        {
                            Thread.Sleep(250);
                            continue;
                        }
                    }

                    byte[] imgData = webWizard.DownloadData(imgSrc);

                    Image image = new()
                    {
                        Listing = listing,
                        Data = imgData,
                        ContentType = DetectMediaType(imgData)
                    };

                    yield return image;
                }

                IWebElement closeImages = DriverBehavior.FindElement(driver, By.CssSelector("button.fullscreen-action-bar__close-button"));
                closeImages.Click();
            }
        }
    }
}
