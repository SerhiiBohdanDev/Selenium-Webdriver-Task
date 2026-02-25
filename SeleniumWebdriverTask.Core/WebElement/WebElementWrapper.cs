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

public class WebElementWrapper
{
    // passing true aligns top of the element with the top of the view
    private const string JavascriptScrollCommand = "arguments[0].scrollIntoView(true);";
    private const string JavascriptClickCommand = "arguments[0].click();";

    private readonly DriverWrapper _driverWrapper;
    private readonly IWebElement _element;

    public WebElementWrapper(DriverWrapper driverWrapper, IWebElement element)
    {
        _driverWrapper = driverWrapper;
        _element = element;
    }

    public string? TextContent => _element.GetAttribute("textContent");

    public string? Href => _element.GetAttribute("href");

    public string Text => _element.Text;

    private IWebDriver WebDriver => _driverWrapper.WebDriver;

    private WebDriverWait Wait => _driverWrapper.Wait;

    public void SafeClick()
    {
        new Actions(WebDriver)
            .MoveToElement(_element)
            .Click()
            .Build()
            .Perform();
    }

    public void JavascriptClick()
    {
        WebDriver.ExecuteJavaScript(JavascriptClickCommand, _element);
    }

    public void EnterText(string text)
    {
        _element.SendKeys(text);
    }

    public void PressEnter()
    {
        _element.SendKeys(Keys.Enter);
    }

    public void ScrollToElement()
    {
        WebDriver.ExecuteJavaScript(JavascriptScrollCommand, _element);
    }

    public void Hover()
    {
        new Actions(WebDriver)
            .MoveToElement(_element)
            .Perform();
    }

    public WebElementWrapper WaitUntilDisplayed()
    {
        WaitForCondition(() => _element.Displayed);
        return this;
    }

    public WebElementWrapper WaitUntilEnabled()
    {
        WaitForCondition(() => _element.Displayed && _element.Enabled);
        return this;
    }

    public WebElementWrapper WaitUntilLinkIsReady()
    {
        WaitForCondition(() => _element.Displayed && _element.Enabled && Href != null);
        return this;
    }

    public WebElementWrapper FindElement(By by)
    {
        var element = Waiter.WaitForElements(by, () => _element.FindElement(by), Wait);
        return element.WrapElement(_driverWrapper);
    }

    public ReadOnlyCollection<WebElementWrapper> FindElements(By by)
    {
        var elements = new ReadOnlyCollection<IWebElement>([]);
        Waiter.WaitForElements(
            by,
            () =>
            {
                elements = _element.FindElements(by);
                if (elements.Count == 0)
                {
                    return null;
                }

                return elements.WrapElements(_driverWrapper);
            },
            Wait);

        return elements.WrapElements(_driverWrapper);
    }

    private void WaitForCondition(Func<bool> condition)
    {
        Waiter.WaitForCondition(Wait, condition);
    }
}
