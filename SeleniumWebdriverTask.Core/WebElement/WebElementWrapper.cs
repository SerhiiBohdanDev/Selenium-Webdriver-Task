// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.CoreLayer.WebElement;

internal class WebElementWrapper
{
    // passing true aligns top of the element with the top of the view
    private const string JavascriptScrollCommand = "arguments[0].scrollIntoView(true);";
    private const string JavascriptClickCommand = "arguments[0].click();";

    private readonly IWebDriver _driver;
    private readonly IWebElement _element;

    public WebElementWrapper(IWebDriver driver, IWebElement element)
    {
        _driver = driver;
        _element = element;
    }

    public bool IsDisplayed => _element.Displayed;

    public bool IsEnabled => IsDisplayed && _element.Enabled;

    public bool IsLinkReady => IsEnabled && _element.GetUrl() != null;

    public string? TextContent => _element.GetAttribute("textContent");

    public void SafeClick()
    {
        new Actions(_driver)
            .MoveToElement(_element)
            .Click()
            .Build()
            .Perform();
    }

    public void JavascriptClick(IWebElement element)
    {
        _driver.ExecuteJavaScript(JavascriptClickCommand, element);
    }

    public void EnterText(string text)
    {
        _element.SendKeys(text);
    }

    public void ScrollToElement()
    {
        _driver.ExecuteJavaScript(JavascriptScrollCommand, _element);
    }

    public void Hover()
    {
        new Actions(_driver)
            .MoveToElement(_element)
            .Perform();
    }
}
