using OpenQA.Selenium;
using HemnetCrawler.Domain;

namespace HemnetCrawler.ConsoleApp.PageInteractives
{
    internal static class StartPage
    {
        public static void EnterHemnet(IWebDriver driver)
        {
            driver.Url = "https://www.hemnet.se/";
            driver.Navigate();
            driver.Manage().Window.Maximize();

            By acceptCookiesButtonSelector = By.CssSelector(".consent__button-wrapper > button.hcl-button--primary");
            DriverBehavior.TryFindElement(driver, acceptCookiesButtonSelector).Click();
        }

        public static void AddSearchBase(IWebDriver driver, ILogger logger)
        {
            string county1 = "Västra Götalands län";
            string county2 = "Hallands län";

            string[] counties = { county1, county2 };

            By searchBoxSelector = By.CssSelector("#area-search-input-box");
            IWebElement searchBox = DriverBehavior.TryFindElement(driver, searchBoxSelector);
            searchBox.Click();

            for (int i = 0; i < counties.Length; i++)
            {
                searchBox.SendKeys(counties[i]);

                By countySearchFilterKeywordSelector = By.CssSelector(".item-first.item.alt");
                DriverBehavior.TryFindElement(driver, countySearchFilterKeywordSelector).Click();

                searchBox.Click();
            }

            By submitButtonSelector = By.CssSelector(".js-submit-button.js-show-on-forsale");
            DriverBehavior.TryFindElement(driver, submitButtonSelector).Click();

            logger.Log($"Searching within counties: {string.Join(", ", counties)}.");
        }
    }
}
