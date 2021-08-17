using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HemnetCrawler.ConsoleApp
{
    internal class DriverBehavior
    {
        public static IWebElement TryFindElement(IWebDriver driver, By findBy)
        {
            DateTime nextTryStart = DateTime.Now;

            while (true)
            {
                DateTime nextTryProgressed = DateTime.Now;

                try
                {
                    IWebElement element = driver.FindElement(findBy);
                    return element;
                }
                catch (NoSuchElementException)
                {
                    if (nextTryProgressed.Subtract(nextTryStart).Seconds < 15)
                    {
                        throw;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        nextTryStart = DateTime.Now;
                    }
                }
            }
        }

        public static IReadOnlyCollection<IWebElement> TryFindElements(IWebDriver driver, By findBy)
        {
            DateTime nextTryStart = DateTime.Now;

            while (true)
            {
                DateTime nextTryProgressed = DateTime.Now;

                try
                {
                    driver.FindElement(findBy);

                    return driver.FindElements(findBy);
                }
                catch (NoSuchElementException)
                {
                    if (nextTryProgressed.Subtract(nextTryStart).Seconds < 15)
                    {
                        throw;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        nextTryStart = DateTime.Now;
                    }
                }
            }
        }

        public static IWebElement TryFindElementInsideElement(IWebElement container, By findBy)
        {
            DateTime nextTryStart = DateTime.Now;

            while (true)
            {
                DateTime nextTryProgressed = DateTime.Now;

                try
                {
                    IWebElement element = container.FindElement(findBy);
                    return element;
                }
                catch (NoSuchElementException)
                {
                    if (nextTryProgressed.Subtract(nextTryStart).Seconds < 15)
                    {
                        throw;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        nextTryStart = DateTime.Now;
                    }
                }
            }
        }

        public static IReadOnlyCollection<IWebElement> TryFindElementsInsideElement(IWebElement container, By findBy)
        {
            DateTime nextTryStart = DateTime.Now;

            while (true)
            {
                DateTime nextTryProgressed = DateTime.Now;

                try
                {
                    container.FindElement(findBy);
                    return container.FindElements(findBy);
                }
                catch (NoSuchElementException)
                {
                    if (nextTryProgressed.Subtract(nextTryStart).Seconds < 15)
                    {
                        throw;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        nextTryStart = DateTime.Now;
                    }
                }
            }
        }
    }
}
