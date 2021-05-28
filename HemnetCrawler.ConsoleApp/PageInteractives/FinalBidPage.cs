﻿using HemnetCrawler.Data;
using HemnetCrawler.Data.Entities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HemnetCrawler.ConsoleApp
{
    public class FinalBidPage
    {
        public static void InterpretTable(FinalBid finalBid, Dictionary<string, string> labelsAndValues)
        {
            foreach (KeyValuePair<string, string> pair in labelsAndValues)
            {
                switch (pair.Key)
                {
                    case "Begärt pris":
                            finalBid.DemandedPrice = Utils.DigitPurist(pair.Value);
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
                        finalBid.Rooms = pair.Value;
                        break;

                    case "Byggår":
                        finalBid.ConstructionYear = pair.Value;
                        break;

                    case "Avgift/månad":
                        string trimmedValue = pair.Value.Trim();

                        if (trimmedValue != "kr/mån" && trimmedValue != "kr/månad")
                            finalBid.Fee = Utils.DigitPurist(pair.Value);

                        break;

                    case "Driftskostnad":
                        finalBid.Utilities = Utils.DigitPurist(pair.Value);
                        break;
                }
            }
        }

        public static void CreateEntity(IWebDriver driver, int hemnetId, FinalBid finalBid)
        {
            finalBid.HemnetId = hemnetId;
            finalBid.LastUpdated = DateTimeOffset.Now;
            finalBid.Street = driver.FindElement(By.CssSelector("h1.hcl-heading")).Text.Replace("Slutpris", "").Trim();
            finalBid.Price = Utils.DigitPurist(driver.FindElement(By.CssSelector("span.sold-property__price-value")).Text);
            finalBid.SoldDate = DateTimeOffset.Parse(driver.FindElement(By.CssSelector("time")).GetAttribute("datetime"));

            Regex postalCodePattern1 = new Regex("(?<=\"property_zipcode\",\\s\")\\d{5}");
            Regex postalCodePattern2 = new Regex("(?<=\"property_zipcode\",\\s\")\\d{3}\\s\\d{2}");
            string postalCode = postalCodePattern1.IsMatch(driver.PageSource) ?
                                postalCodePattern1.Match(driver.PageSource).Value :
                                postalCodePattern2.Match(driver.PageSource).Value;
            finalBid.PostalCode = int.Parse(postalCode);

            Regex redundantWhitespace = new Regex("\\s{2,}");
            string city = driver.FindElement(By.CssSelector("p.sold-property__metadata.qa-sold-property-metadata")).Text.Replace("\n", "");
            city = redundantWhitespace.Replace(city, " ");
            finalBid.City = city.Split("-")[1].Trim();

            var labels = driver.FindElements(By.CssSelector("dt.sold-property__attribute")).Select(e => e.Text).ToList();
            var values = driver.FindElements(By.CssSelector("dd.sold-property__attribute-value")).Select(e => e.Text).ToList();

            var labelsAndValues = new Dictionary<string, string>();
            for (int i = 0; i < labels.Count; i++)
            {
                if (!String.IsNullOrEmpty(labels[i]))
                    labelsAndValues.Add(labels[i], values[i]);
            }

            FinalBidPage.InterpretTable(finalBid, labelsAndValues);
        }

        public static void CreateFinalBidRecords(IWebDriver driver, int hemnetId, HemnetCrawlerDbContext context)
        {
            FinalBid finalBid = new FinalBid();
            CreateEntity(driver, hemnetId, finalBid);
            context.Add(finalBid);
            context.SaveChanges();
        }
    }
}
