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

        public static IWebElement FindElement(IWebElement container, By findBy, bool nullAllowed = false)
        {
            return TryFindElement(container.FindElement, findBy, nullAllowed);
        }

        public static IWebElement FindElement(IWebDriver driver, By findBy, bool nullAllowed = false)
        {
            return TryFindElement(driver.FindElement, findBy, nullAllowed);
        }

        public static IReadOnlyCollection<IWebElement> FindElements(IWebElement container, By findBy)
        {
            return TryFindElements(container.FindElements, findBy);
        }

        public static IReadOnlyCollection<IWebElement> FindElements(IWebDriver driver, By findBy)
        {
            return TryFindElements(driver.FindElements, findBy);
        }

        // A. Vid null ska det smälla.
        // B. Kan returnera null.

        // C. Om föregående element var null så ska detta inte eftersökas alls.

        private static IWebElement TryFindElement(Func<By, IWebElement> findElement, By findBy, bool nullAllowed)
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
                        if (nullAllowed) return null;
                        else throw new Exception("No such elements found.");
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
                Thread.Sleep(1000);

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
