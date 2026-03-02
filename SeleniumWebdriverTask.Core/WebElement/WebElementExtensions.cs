// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Collections.ObjectModel;
using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.CoreLayer.WebElement;

/// <summary>
/// A class with IWebElement related extensions.
/// </summary>
internal static class WebElementExtensions
{
    /// <summary>
    /// Wraps IWebElement into WebElementWrapper.
    /// </summary>
    /// <param name="element">Element that needs to be wrapped.</param>
    /// <param name="driverWrapper">DriverWrapper instance.</param>
    /// <returns>Instance of WebElementWrapper.</returns>
    public static WebElementWrapper WrapElement(this IWebElement element, DriverWrapper driverWrapper)
    {
        return new WebElementWrapper(driverWrapper, element);
    }

    /// <summary>
    /// Wraps a collection of IWebElement into collection of WebElementWrapper.
    /// </summary>
    /// <param name="elements">Collection that needs to be wrapped.</param>
    /// <param name="driverWrapper">DriverWrapper instance.</param>
    /// <returns>A collection of WebELementWrapper, empty collection if original collection was empty.</returns>
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
