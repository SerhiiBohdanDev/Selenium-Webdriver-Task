// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.CoreLayer.WebElement.Elements;

/// <summary>
/// Class representing a text element.
/// </summary>
public class TextElement : WebElementWrapper
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextElement"/> class.
    /// </summary>
    /// <param name="driverWrapper">Instance of the DriverWrapper.</param>
    /// <param name="element">Element that is being wrapped.</param>
    public TextElement(DriverWrapper driverWrapper, IWebElement element)
        : base(driverWrapper, element)
    {
    }
}
