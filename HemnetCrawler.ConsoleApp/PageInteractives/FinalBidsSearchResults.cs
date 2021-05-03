using HemnetCrawler.Data;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace HemnetCrawler.ConsoleApp
{
    public class FinalBidsSearchResults
    {
        public static void SpecifyAndSortResults(IWebDriver driver)
        {
            driver.FindElements(By.CssSelector("div.result-type-toggle__label")).Where(e => e.Text == "Slutpriser").First().Click();
            Thread.Sleep(2000);

            var sortBy = driver.FindElement(By.CssSelector("#search-results-sort-by"));
            var options = sortBy.FindElements(By.CssSelector("option"));

            sortBy.Click();
            options.Where(e => e.Text == "Tidigast såld/överlåten först").First().Click();
        }

        public static void AddAgeFilter(IWebDriver driver)
        {
            HemnetCrawlerDbContext context = new HemnetCrawlerDbContext();

            if (context.FinalBids.Count() > 0)
            {
                DateTimeOffset latestPublish = context.FinalBids.Select(finalBid => finalBid.SoldDate).Max();
                int daysDiff = (int)Math.Ceiling(Utils.GetTotalDays(latestPublish, DateTimeOffset.Now));

                string ageSearchFilter;

                if (daysDiff <= 84)
                {
                    ageSearchFilter = "search_sold_age_3m";
                }
                else if (daysDiff <= 168)
                {
                    ageSearchFilter = "search_sold_age_6m";
                }
                else if (daysDiff <= 336)
                {
                    ageSearchFilter = "search_sold_age_12m";
                }
                else
                {
                    ageSearchFilter = "search_sold_age_all";
                }

                driver.FindElements(By.CssSelector("label.radio-token-list__label")).Where(e => e.GetAttribute("for") == $"{ageSearchFilter}").First().Click();
                Thread.Sleep(3000);

                driver.FindElement(By.CssSelector("button.search-form__submit-button")).Click();
                Thread.Sleep(3000);
            }
        }
    }
}
