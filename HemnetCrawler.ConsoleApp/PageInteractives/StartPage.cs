using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HemnetCrawler.ConsoleApp
{
    public static class StartPage
    {
        public static void EnterHemnet(IWebDriver driver)
        {
            driver.Url = "https://www.hemnet.se/";
            driver.Navigate();
            driver.Manage().Window.Maximize();
            Thread.Sleep(1000);

            IWebElement acceptCookiesButton = driver.FindElement(By.CssSelector("button.hcl-button--primary"));
            acceptCookiesButton.Click();
            Thread.Sleep(1000);
        }

        public static void AddSearchBase(IWebDriver driver)
        {
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
            Thread.Sleep(7000);
        }
    }
}
