// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using OpenQA.Selenium;
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
}
