// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebdriverTask.CoreLayer.Utils;

/// <summary>
/// Helper class to find elements.
/// </summary>
internal static class ElementsFinder
{
    /// <summary>
    /// Waits and finds element.
    /// </summary>
    /// <param name="by">Locator to find element by.</param>
    /// <param name="searchContext">Driver or element.</param>
    /// <param name="wait">WebDriverWait instance.</param>
    /// <returns>Instance of IWebELement.</returns>
    public static IWebElement FindElement(By by, ISearchContext searchContext, WebDriverWait wait)
    {
        return Waiter.WaitForElements(by, () => searchContext.FindElement(by), wait);
    }

    /// <summary>
    /// Waits and finds elements.
    /// </summary>
    /// <param name="by">Locator to find elements by.</param>
    /// <param name="searchContext">Driver or element.</param>
    /// <param name="wait">WebDriverWait instance.</param>
    /// <returns>Collection of elements or empty collection if none were found.</returns>
    public static ReadOnlyCollection<IWebElement> FindElements(By by, ISearchContext searchContext, WebDriverWait wait)
    {
        var elements = new ReadOnlyCollection<IWebElement>([]);
        Waiter.WaitForElements(
            by,
            () =>
            {
                elements = searchContext.FindElements(by);
                if (elements.Count == 0)
                {
                    return null;
                }

                return elements;
            },
            wait);

        return elements;
    }
}
