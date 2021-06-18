using OpenQA.Selenium;
using System.Threading;
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
            Thread.Sleep(1000);

            IWebElement acceptCookiesButton = driver.FindElement(By.CssSelector(".consent__button-wrapper > button.hcl-button--primary"));
            acceptCookiesButton.Click();
            Thread.Sleep(1000);
        }

        public static void AddSearchBase(IWebDriver driver, ILogger logger)
        {
            string county1 = "Västra Götalands län";
            string county2 = "Hallands län";

            string[] counties = { county1, county2 };

            string countiesString = "";

            IWebElement searchBox = driver.FindElement(By.CssSelector("#area-search-input-box"));
            searchBox.Click();
            Thread.Sleep(1000);

            searchBox.SendKeys(counties[0]);
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector(".item-first.item.alt")).Click();
            searchBox.Click();
            Thread.Sleep(1000);

            searchBox.SendKeys(counties[1]);
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector(".item-first.item.alt")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector(".js-submit-button.js-show-on-forsale")).Click();
            Thread.Sleep(7000);

            for (int i = 0; i < counties.Length; i++)
            {
                countiesString += i < counties.Length - 1 ? 
                                       counties[i] + ", " : 
                                       counties[i];
            }

            logger.Log($"Searching within counties: {countiesString}.");
        }
    }
}
