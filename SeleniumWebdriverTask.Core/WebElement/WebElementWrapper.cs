// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumWebdriverTask.CoreLayer.Utils;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.CoreLayer.WebElement;

/// <summary>
/// A wrapper class for IWebDriver.
/// </summary>
public class WebElementWrapper
{
    // passing true aligns top of the element with the top of the view
    private const string JavascriptScrollCommand = "arguments[0].scrollIntoView(true);";
    private const string JavascriptClickCommand = "arguments[0].click();";

    private readonly DriverWrapper _driverWrapper;
    private readonly IWebElement _element;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebElementWrapper"/> class.
    /// </summary>
    /// <param name="driverWrapper">Instance of the DriverWrapper.</param>
    /// <param name="element">Element that is being wrapped.</param>
    public WebElementWrapper(DriverWrapper driverWrapper, IWebElement element)
    {
        _driverWrapper = driverWrapper;
        _element = element;
    }

    /// <summary>
    /// Gets text content of the element by attribute.
    /// </summary>
    public string? TextContent => _element.GetAttribute("textContent");

    /// <summary>
    /// Gets element's href attribute.
    /// </summary>
    public string? Href => _element.GetAttribute("href");

    /// <summary>
    /// Gets element's text.
    /// </summary>
    public string Text => _element.Text;

    private IWebDriver WebDriver => _driverWrapper.WebDriver;

    private WebDriverWait Wait => _driverWrapper.Wait;

    /// <summary>
    /// Moves mouse to element and clicks it.
    /// </summary>
    public void SafeClick()
    {
        new Actions(WebDriver)
            .MoveToElement(_element)
            .Click()
            .Build()
            .Perform();
    }

    /// <summary>
    /// Uses javascript to click the element.
    /// </summary>
    public void JavascriptClick()
    {
        WebDriver.ExecuteJavaScript(JavascriptClickCommand, _element);
    }

    /// <summary>
    /// Sends text to the element.
    /// </summary>
    /// <param name="text">Text to send.</param>
    public void EnterText(string text)
    {
        _element.SendKeys(text);
    }

    /// <summary>
    /// Emulates pressing Enter.
    /// </summary>
    public void PressEnter()
    {
        _element.SendKeys(Keys.Enter);
    }

    /// <summary>
    /// Scrolls to the element using javascript.
    /// </summary>
    public void ScrollToElement()
    {
        WebDriver.ExecuteJavaScript(JavascriptScrollCommand, _element);
    }

    /// <summary>
    /// Emulates hovering mouse over the element.
    /// </summary>
    public void Hover()
    {
        new Actions(WebDriver)
            .MoveToElement(_element)
            .Perform();
    }

    /// <summary>
    /// Waits until element is displayed.
    /// </summary>
    /// <returns>Instance of the WebElementWrapper.</returns>
    public WebElementWrapper WaitUntilDisplayed()
    {
        Waiter.WaitForCondition(Wait, () => _element.Displayed);
        return this;
    }

    /// <summary>
    /// Waits until element is enabled.
    /// </summary>
    /// <returns>Instance of the WebElementWrapper.</returns>
    public WebElementWrapper WaitUntilEnabled()
    {
        Waiter.WaitForCondition(Wait, () => _element.Displayed && _element.Enabled);
        return this;
    }

    /// <summary>
    /// Waits until link is ready.
    /// </summary>
    /// <returns>Instance of the WebElementWrapper.</returns>
    public WebElementWrapper WaitUntilLinkIsReady()
    {
        Waiter.WaitForCondition(Wait, () => _element.Displayed && _element.Enabled && Href != null);
        return this;
    }

    /// <summary>
    /// Finds an element.
    /// </summary>
    /// <param name="by">Element locator.</param>
    /// <returns>Instance of the WebElementWrapper.</returns>
    public WebElementWrapper FindElement(By by)
    {
        var element = ElementsFinder.FindElement(by, _element, Wait);
        return element.WrapElement(_driverWrapper);
    }

    /// <summary>
    /// Finds elements.
    /// </summary>
    /// <param name="by">Element locator.</param>
    /// <returns>A collection of WebElementWrapper, or an empty collection if none were found..</returns>
    public ReadOnlyCollection<WebElementWrapper> FindElements(By by)
    {
        var elements = ElementsFinder.FindElements(by, _element, Wait);
        return elements.WrapElements(_driverWrapper);
    }
}
