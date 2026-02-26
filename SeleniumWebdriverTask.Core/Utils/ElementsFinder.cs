// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebdriverTask.CoreLayer.Utils;

internal static class ElementsFinder
{
    public static IWebElement FindElement(By by, ISearchContext searchContext, WebDriverWait wait)
    {
        return Waiter.WaitForElements(by, () => searchContext.FindElement(by), wait);
    }

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
