// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Collections.ObjectModel;
using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.CoreLayer.WebElement;

namespace SeleniumWebdriverTask.CoreLayer.Utils;

internal static class ElementsFinder
{
    public static WebElementWrapper FindAndWrapElement(By by, ISearchContext searchContext, DriverWrapper driverWrapper)
    {
        var element = Waiter.WaitForElements(by, () => searchContext.FindElement(by), driverWrapper.Wait);
        return element.WrapElement(driverWrapper);
    }

    public static ReadOnlyCollection<WebElementWrapper> FindAndWrapElements(By by, ISearchContext searchContext, DriverWrapper driverWrapper)
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

                return elements.WrapElements(driverWrapper);
            },
            driverWrapper.Wait);

        return elements.WrapElements(driverWrapper);
    }
}
