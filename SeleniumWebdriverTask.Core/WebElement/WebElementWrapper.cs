// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
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
        return new WebElementWrapper(_driverWrapper, WaitForElements(by, () => _element.FindElement(by)));
    }

    public ReadOnlyCollection<WebElementWrapper> FindElements(By by)
    {
        var elements = new ReadOnlyCollection<IWebElement>([]);
        WaitForElements(by, () =>
        {
            elements = _element.FindElements(by);
            if (elements.Count == 0)
            {
                return null;
            }

            return WrapElements(elements);
        });

        return WrapElements(elements);
    }

    private ReadOnlyCollection<WebElementWrapper> WrapElements(ReadOnlyCollection<IWebElement> elements)
    {
        var wrappedElements = new List<WebElementWrapper>();
        for (int i = 0; i < elements.Count; i++)
        {
            wrappedElements.Add(new WebElementWrapper(_driverWrapper, elements[i]));
        }

        return wrappedElements.AsReadOnly();
    }

    private void WaitForCondition(Func<bool> condition)
    {
        var retries = 0;
        var result = false;
        while (retries < DriverWrapper.MaxRetries)
        {
            try
            {
                _driverWrapper.Wait.Until(driver =>
                {
                    result = condition.Invoke();
                    return result;
                });
            }
            catch (WebDriverTimeoutException)
            {
                retries++;
            }

            if (result)
            {
                break;
            }
        }

        if (!result)
        {
            throw new WebDriverTimeoutException($"Driver timed out after {DriverWrapper.MaxRetries} retries.");
        }
    }

    /// <summary>
    /// A wrapper method to allow finding one IWebElement or ReadOnlyCollection of type IWebElement.
    /// </summary>
    /// <typeparam name="T">IWebElement or ReadOnlyCollection of type IWebElement.</typeparam>
    /// <param name="by">Locator to reference in exception.</param>
    /// <param name="findAction">Action to find elements and check if they fit the required condition.</param>
    /// <returns>One IWebElement or ReadOnlyCollection(IWebElement).</returns>
    /// <exception cref="NoSuchElementException">Thrown if element was not found when looking for a single element.</exception>
    /// <exception cref="StaleElementReferenceException">Thrown if an element or collection were stale.</exception>
    private T WaitForElements<T>(By by, Func<T> findAction)
    {
        var retries = 0;
        var exceptionCaught = typeof(NoSuchElementException);
        while (retries < DriverWrapper.MaxRetries)
        {
            try
            {
                return _driverWrapper.Wait.Until(driver =>
                {
                    try
                    {
                        return findAction.Invoke();
                    }
                    catch (StaleElementReferenceException)
                    {
                        exceptionCaught = typeof(StaleElementReferenceException);
                        return default;
                    }
                    catch (NoSuchElementException)
                    {
                        exceptionCaught = typeof(NoSuchElementException);
                        return default;
                    }
                });
            }
            catch (WebDriverTimeoutException)
            {
                retries++;
            }
        }

        if (exceptionCaught == typeof(NoSuchElementException))
        {
            throw new NoSuchElementException($"Could not find element located by {by} after {DriverWrapper.MaxRetries} attempts.");
        }
        else
        {
            throw new StaleElementReferenceException($"Element located by {by} remained stale after {DriverWrapper.MaxRetries} attempts.");
        }
    }
}
