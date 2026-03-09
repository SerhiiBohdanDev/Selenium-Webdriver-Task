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
/// A wrapper class for IWebElement.
/// </summary>
public class WebElementWrapper
{
    // passing true aligns top of the element with the top of the view
    private const string JavascriptScrollCommand = "arguments[0].scrollIntoView(true);";
    private const string JavascriptClickCommand = "arguments[0].click();";

    private readonly DriverWrapper _driverWrapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebElementWrapper"/> class.
    /// </summary>
    /// <param name="driverWrapper">Instance of the DriverWrapper.</param>
    /// <param name="element">Element that is being wrapped.</param>
    public WebElementWrapper(DriverWrapper driverWrapper, IWebElement element)
    {
        _driverWrapper = driverWrapper;
        Element = element;
    }

    /// <summary>
    /// Gets a value indicating whether or not element is displayed.
    /// </summary>
    public bool IsDisplayed => Element.Displayed;

    /// <summary>
    /// Gets text content of the element by attribute.
    /// </summary>
    public string? TextContent => Element.GetAttribute("textContent");

    /// <summary>
    /// Gets element's text.
    /// </summary>
    public string Text => Element.Text;

    /// <summary>
    /// Gets inner IWebElement.
    /// </summary>
    protected IWebElement Element { get; private set; }

    /// <summary>
    /// Gets WebDriverWait instance.
    /// </summary>
    protected WebDriverWait Wait => _driverWrapper.Wait;

    private IWebDriver WebDriver => _driverWrapper.WebDriver;

    /// <summary>
    /// Moves mouse to element and clicks it.
    /// </summary>
    public void SafeClick()
    {
        new Actions(WebDriver)
            .MoveToElement(Element)
            .Click()
            .Build()
            .Perform();
    }

    /// <summary>
    /// Uses javascript to click the element.
    /// </summary>
    public void JavascriptClick()
    {
        WebDriver.ExecuteJavaScript(JavascriptClickCommand, Element);
    }

    /// <summary>
    /// Scrolls to the element using javascript.
    /// </summary>
    public void ScrollToElement()
    {
        WebDriver.ExecuteJavaScript(JavascriptScrollCommand, Element);
    }

    /// <summary>
    /// Emulates hovering mouse over the element.
    /// </summary>
    public void Hover()
    {
        new Actions(WebDriver)
            .MoveToElement(Element)
            .Perform();
    }

    /// <summary>
    /// Waits until element is displayed.
    /// </summary>
    public void WaitUntilDisplayed()
    {
        Waiter.WaitForCondition(Wait, () => Element.Displayed);
    }

    /// <summary>
    /// Waits until element is enabled.
    /// </summary>
    public void WaitUntilEnabled()
    {
        Waiter.WaitForCondition(Wait, () => Element.Displayed && Element.Enabled);
    }

    /// <summary>
    /// Finds and wraps an element.
    /// </summary>
    /// <typeparam name="T">WebElementWrapper type.</typeparam>
    /// <param name="by">Element locator.</param>
    /// <returns>Instance of WebElementWrapper.</returns>
    public T FindElement<T>(By by)
        where T : WebElementWrapper
    {
        var element = ElementsFinder.FindElement(by, Element, Wait);
        return element.WrapElement<T>(_driverWrapper);
    }

    /// <summary>
    /// Finds elements.
    /// </summary>
    /// <typeparam name="T">WebElementWrapper type.</typeparam>
    /// <param name="by">Element locator.</param>
    /// <returns>A collection of WebElementWrapper, or an empty collection if none were found..</returns>
    public ReadOnlyCollection<T> FindElements<T>(By by)
         where T : WebElementWrapper
    {
        var elements = ElementsFinder.FindElements(by, Element, Wait);
        return elements.WrapElements<T>(_driverWrapper);
    }
}
