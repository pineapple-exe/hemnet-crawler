using HemnetCrawler.Domain;
using HemnetCrawler.Domain.Entities;
using HemnetCrawler.Domain.Repositories;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HemnetCrawler.ConsoleApp.PageInteractives
{
    internal class FinalBidPage
    {
        private static FinalBid InterpretTable(FinalBid finalBid, Dictionary<string, string> labelsAndValues)
        {
            foreach (KeyValuePair<string, string> pair in labelsAndValues)
            {
                switch (pair.Key)
                {
                    case "Begärt pris":
                            finalBid.DemandedPrice = Utils.NumberPurist(pair.Value);
                        break;

                    case "Prisutveckling":
                        finalBid.PriceDevelopment = pair.Value;
                        break;

                    case "Bostadstyp":
                        finalBid.HomeType = pair.Value;
                        break;

                    case "Upplåtelseform":
                        finalBid.OwnershipType = pair.Value;
                        break;

                    case "Antal rum":
                        finalBid.Rooms = Utils.NumberPuristDouble(pair.Value);
                        break;

                    case "Boarea":
                        finalBid.LivingArea = Utils.NumberPurist(pair.Value);
                        break;

                    case "Biarea":
                        finalBid.BiArea = Utils.NumberPurist(pair.Value);
                        break;

                    case "Tomtarea":
                        finalBid.PropertyArea = Utils.NumberPurist(pair.Value);
                        break;

                    case "Byggår":
                        finalBid.ConstructionYear = pair.Value;
                        break;

                    case "Avgift/månad":
                        string trimmedValue = pair.Value.Trim();

                        if (trimmedValue != "kr/mån" && trimmedValue != "kr/månad")
                            finalBid.Fee = Utils.NumberPurist(pair.Value);

                        break;

                    case "Driftskostnad":
                        finalBid.Utilities = Utils.NumberPurist(pair.Value);
                        break;
                }
            }
            return finalBid;
        }

        private static FinalBid CreateEntity(IWebDriver driver, int hemnetId)
        {
            FinalBid finalBid = new();

            finalBid.HemnetId = hemnetId;
            finalBid.Href = driver.Url;
            finalBid.LastUpdated = DateTimeOffset.Now;
            finalBid.Street = DriverBehavior.FindElement(driver, By.CssSelector("h1.hcl-heading")).Text.Replace("Slutpris", "").Trim();
            finalBid.Price = Utils.NumberPurist(DriverBehavior.FindElement(driver, By.CssSelector("span.sold-property__price-value")).Text);
            finalBid.SoldDate = DateTime.Parse(DriverBehavior.FindElement(driver, By.CssSelector("time")).GetAttribute("datetime"));

            Regex postalCodePattern = new("(?<=\"property_zipcode\",\\s\")\\d{3}\\s?\\d{2}");
            string postalCode = postalCodePattern.Match(driver.PageSource).Value;
            if (postalCode != "")
                finalBid.PostalCode = Utils.NumberPurist(postalCode);

            Regex redundantWhitespace = new("\\s{2,}");
            string city = DriverBehavior.FindElement(driver, By.CssSelector("p.sold-property__metadata.qa-sold-property-metadata")).Text.Replace("\n", "");
            city = redundantWhitespace.Replace(city, " ");
            finalBid.City = city.Split("-")[1].Trim();

            var labels = DriverBehavior.FindElements(driver, By.CssSelector("dt.sold-property__attribute")).Select(e => e.Text).ToList();
            var values = DriverBehavior.FindElements(driver, By.CssSelector("dd.sold-property__attribute-value")).Select(e => e.Text).ToList();

            var labelsAndValues = new Dictionary<string, string>();
            for (int i = 0; i < labels.Count; i++)
            {
                if (!String.IsNullOrEmpty(labels[i]))
                    labelsAndValues.Add(labels[i], values[i]);
            }

            return FinalBidPage.InterpretTable(finalBid, labelsAndValues);
        }

        public static FinalBid CreateFinalBidRecord(IWebDriver driver, IFinalBidRepository repository, int hemnetId, ILogger logger)
        {
            FinalBid finalBid = CreateEntity(driver, hemnetId);

            repository.AddFinalBid(finalBid);

            logger.Log($"A new Final Bid with Id {finalBid.Id} was created. Located on {finalBid.Street}, {finalBid.City}.");

            return finalBid;
        }
    }
}
