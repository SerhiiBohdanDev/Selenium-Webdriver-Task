// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.CoreLayer.WebElement.Elements;

/// <summary>
/// Class representing a text input field.
/// </summary>
public class TextInput : WebElementWrapper
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextInput"/> class.
    /// </summary>
    /// <param name="driverWrapper">Instance of the DriverWrapper.</param>
    /// <param name="element">Element that is being wrapped.</param>
    public TextInput(DriverWrapper driverWrapper, IWebElement element)
        : base(driverWrapper, element)
    {
    }

    /// <summary>
    /// Sends text to the element.
    /// </summary>
    /// <param name="text">Text to send.</param>
    public void EnterText(string text)
    {
        Element.SendKeys(text);
    }

    /// <summary>
    /// Emulates pressing Enter.
    /// </summary>
    public void PressEnter()
    {
        Element.SendKeys(Keys.Enter);
    }
}
