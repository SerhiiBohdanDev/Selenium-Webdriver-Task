// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWebdriverTask.CoreLayer.Utils;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.CoreLayer.WebElement.Elements;

/// <summary>
/// Class representing a link element.
/// </summary>
public class Link : WebElementWrapper
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Link"/> class.
    /// </summary>
    /// <param name="driverWrapper">Instance of the DriverWrapper.</param>
    /// <param name="element">Element that is being wrapped.</param>
    public Link(DriverWrapper driverWrapper, IWebElement element)
        : base(driverWrapper, element)
    {
    }

    /// <summary>
    /// Gets element's href attribute.
    /// </summary>
    public string? Href => Element.GetAttribute("href");

    /// <summary>
    /// Waits until link is ready.
    /// </summary>
    /// <returns>Instance of the WebElementWrapper.</returns>
    public Link WaitUntilLinkIsReady()
    {
        Waiter.WaitForCondition(Wait, () => Element.Displayed && Element.Enabled && Href != null);
        return this;
    }
}
