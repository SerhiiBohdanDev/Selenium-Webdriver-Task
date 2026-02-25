// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Collections.ObjectModel;
using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.CoreLayer.WebElement;

internal static class WebElementExtensions
{
    public static WebElementWrapper WrapElement(this IWebElement element, DriverWrapper driverWrapper)
    {
        return new WebElementWrapper(driverWrapper, element);
    }

    public static ReadOnlyCollection<WebElementWrapper> WrapElements(this ReadOnlyCollection<IWebElement> elements, DriverWrapper driverWrapper)
    {
        var wrappedElements = new List<WebElementWrapper>();
        for (var i = 0; i < elements.Count; i++)
        {
            wrappedElements.Add(new WebElementWrapper(driverWrapper, elements[i]));
        }

        return wrappedElements.AsReadOnly();
    }
}
