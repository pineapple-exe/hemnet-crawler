using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HemnetCrawler.ConsoleApp
{
    internal class DriverBehavior
    {

        public static void Scroll(IWebDriver driver, string onElementSelector, int xPosition, int yPosition)
        {
            string script = $"document.querySelector('{onElementSelector}').scroll({xPosition}, {yPosition})";
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;

            jsExecutor.ExecuteScript(script);
            Thread.Sleep(100);
        }

        public static IWebElement FindElement(IWebElement container, By findBy)
        {
            return TryFindElement(container.FindElement, findBy);
        }

        public static IWebElement FindElement(IWebDriver driver, By findBy)
        {
            return TryFindElement(driver.FindElement, findBy);
        }

        public static IReadOnlyCollection<IWebElement> FindElements(IWebElement container, By findBy)
        {
            return TryFindElements(container.FindElements, findBy);
        }

        public static IReadOnlyCollection<IWebElement> FindElements(IWebDriver driver, By findBy)
        {
            return TryFindElements(driver.FindElements, findBy);
        }

        private static IWebElement TryFindElement(Func<By, IWebElement> findElement, By findBy)
        {
            DateTime start = DateTime.Now;

            while (true)
            {
                DateTime progressed = DateTime.Now;

                try
                {
                    IWebElement element = findElement(findBy);
                    Thread.Sleep(1000);

                    return element;
                }
                catch (NoSuchElementException)
                {
                    if (progressed.Subtract(start).TotalSeconds > 15)
                    {
                        return null;
                    }
                    else
                    {
                        Thread.Sleep(250);
                    }
                }
            }
        }

        public static IReadOnlyCollection<IWebElement> TryFindElements(Func<By, IReadOnlyCollection<IWebElement>> findElements, By findBy)
        {
            IReadOnlyCollection<IWebElement> elements;
            DateTime start = DateTime.Now;

            while (true)
            {
                DateTime progressed = DateTime.Now;

                elements = findElements(findBy);

                if (elements.Count == 0)
                {
                    if (progressed.Subtract(start).TotalSeconds > 15)
                    {
                        throw new Exception("No such elements found.");
                    }
                    else
                    {
                        Thread.Sleep(250);
                    }
                }
                else
                {
                    return elements;
                }
            }
        }
    }
}
