using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace LocatorsForWebElements.CoreLayer;

public class DriverWrapper
{
    // passing true aligns top of the element with the top of the view
    private const string JavascriptScrollCommand = "arguments[0].scrollIntoView(true);";
    private const string JavascriptClickCommand = "arguments[0].click();";

    private const int MaxRetries = 3;
    private readonly IWebDriver _driver;
    private readonly TimeSpan _timeout;

    public DriverWrapper(IWebDriver driver, TimeSpan timeout)
    {
        _driver = driver;
        _timeout = timeout;
    }

    public string PageSource => _driver.PageSource;

    public string Url => _driver.Url;

    public ILogs Logs => _driver.Manage().Logs;

    public static string? GetElementText(IWebElement element) => element.GetText();

    public static async Task<bool> WaitForFileToFinishChangingContentAsync(string filePath, int pollingIntervalInSeconds, CancellationToken cancellationToken)
    {
        await WaitForFileToExistAsync(filePath, pollingIntervalInSeconds, cancellationToken);

        var fileSize = new FileInfo(filePath).Length;
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }

            await Task.Delay(TimeSpan.FromSeconds(pollingIntervalInSeconds), cancellationToken);

            var newFileSize = new FileInfo(filePath).Length;
            if (newFileSize == fileSize)
            {
                return true;
            }

            fileSize = newFileSize;
        }
    }

    public void GoToUrl(string url) => _driver.Navigate().GoToUrl(url);

    public void Close()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    /// <summary>
    /// Allows clicking an element safely in cases where it can be interrupted by animation or popups.
    /// </summary>
    /// <param name="element">Element we're trying to click.</param>
    public void SafeClick(IWebElement element)
    {
        new Actions(_driver)
            .MoveToElement(element)
            .Click()
            .Build()
            .Perform();
    }

    public void JavascriptClick(IWebElement element)
    {
        _driver.ExecuteJavaScript(JavascriptClickCommand, element);
    }

    public void Hover(IWebElement element)
    {
        new Actions(_driver)
                .MoveToElement(element)
                .Perform();
    }

    public void ScrollToElement(IWebElement element)
    {
        _driver.ExecuteJavaScript(JavascriptScrollCommand, element);
    }

    public IWebElement FindElement(By by, IWebElement? parent = default)
    {
        return WaitForOneOrManyElements<IWebElement>(by, () =>
        {
            return CheckElementValidity(FindOneElement(by, parent));
        });
    }

    public ReadOnlyCollection<IWebElement> FindElements(By by, IWebElement? parent = default)
    {
        return WaitForOneOrManyElements<ReadOnlyCollection<IWebElement>>(by, () =>
        {
            return CheckElementsCollectionValidity(FindManyElements(by, parent));
        });
    }

    public IWebElement FindDisplayedElement(By by, IWebElement? parent = default)
    {
        return WaitForOneOrManyElements<IWebElement>(by, () =>
        {
            return CheckElementValidity(FindOneElement(by, parent), GetDisplayedElement);
        });
    }

    public IWebElement FindClickableElement(By by, IWebElement? parent = default)
    {
        return WaitForOneOrManyElements<IWebElement>(by, () =>
        {
            return CheckElementValidity(FindOneElement(by, parent), GetClickableElement);
        });
    }

    public ReadOnlyCollection<IWebElement> FindClickableElements(By by, IWebElement? parent = default)
    {
        return WaitForOneOrManyElements<ReadOnlyCollection<IWebElement>>(by, () =>
        {
            return CheckElementsCollectionValidity(FindManyElements(by, parent), GetClickableElement);
        });
    }

    public IWebElement FindClickableLinkElement(By by, IWebElement? parent = default)
    {
        return WaitForOneOrManyElements<IWebElement>(by, () =>
        {
            return CheckElementValidity(FindOneElement(by, parent), GetClickableLinkElement);
        });
    }

    private static async Task WaitForFileToExistAsync(string filePath, int timeoutInSeconds, CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }

            if (File.Exists(filePath))
            {
                break;
            }

            await Task.Delay(TimeSpan.FromSeconds(timeoutInSeconds), cancellationToken);
        }
    }

    private static IWebElement GetDisplayedElement(IWebElement element)
    {
        // accessing element's property forces check for stallness
        // is not null because when finding it NoSuchElementException would be thrown by driver
        // without it last element in the list of found jobs would throw StaleElementReferenceException after all retries
        bool _ = element!.Displayed;
        return element;
    }

    private static IWebElement GetClickableElement(IWebElement element)
    {
        // accessing element's property forces check for stallness
        // is not null because when finding it NoSuchElementException would be thrown by driver
        bool _ = element.Displayed && element.Enabled;
        return element;
    }

    private static IWebElement GetClickableLinkElement(IWebElement element)
    {
        // accessing element's property forces check for stallness
        // is not null because when finding it NoSuchElementException would be thrown by driver
        bool _ = element.Displayed && element.Enabled && element.GetUrl() != null;
        return element;
    }

    private static IWebElement CheckElementValidity(
        IWebElement element,
        Func<IWebElement, IWebElement>? checkAction = null)
    {
        if (checkAction != null)
        {
            return checkAction.Invoke(element);
        }

        return element;
    }

    private static ReadOnlyCollection<IWebElement> CheckElementsCollectionValidity(
        ReadOnlyCollection<IWebElement> elements,
        Func<IWebElement, IWebElement>? checkAction = null)
    {
        // returning null allows running WebDriverWait until we receive the elements
        if (elements.Count == 0)
        {
#pragma warning disable S1168 // Possible null reference return.
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore S1168 // Possible null reference return.
        }

        if (checkAction != null)
        {
            foreach (var item in elements)
            {
                checkAction.Invoke(item);
            }
        }

        return elements;
    }

    /// <summary>
    /// A wrapper method to allow finding one IWebElement or ReadOnlyCollection of type IWebElement.
    /// </summary>
    /// <typeparam name="T">IWebElement or ReadOnlyCollection of type IWebElement.</typeparam>
    /// <param name="by">Locator to reference in exception.</param>
    /// <param name="validityCheckAction">Action to perform on element/elements to check if they fit the condition.</param>
    /// <returns>One IWebElement or ReadOnlyCollection of type IWebElement.</returns>
    /// <exception cref="ArgumentNullException">Thrown if locator is null.</exception>
    /// <exception cref="NoSuchElementException">Thrown if element was not found when looking for a single element.</exception>
    /// <exception cref="StaleElementReferenceException">Thrown if an element or collection were stale.</exception>
    private T WaitForOneOrManyElements<T>(By by, Func<T> validityCheckAction)
    {
        ArgumentNullException.ThrowIfNull(by);

        int retries = 0;
        Type exceptionCaught = typeof(NoSuchElementException);
        while (retries < MaxRetries)
        {
            try
            {
                var wait = new WebDriverWait(_driver, _timeout);
                return wait.Until(driver =>
                {
                    try
                    {
                        return validityCheckAction.Invoke();
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
            throw new NoSuchElementException($"Could not find element located by {by} after {MaxRetries} attempts.");
        }
        else
        {
            throw new StaleElementReferenceException($"Element located by {by} remained stale after {MaxRetries} attempts.");
        }
    }

    private IWebElement FindOneElement(By by, IWebElement? parent = default)
    {
        return parent == null ? _driver.FindElement(by) : parent.FindElement(by);
    }

    private ReadOnlyCollection<IWebElement> FindManyElements(By by, IWebElement? parent = default)
    {
        return parent == null ? _driver.FindElements(by) : parent.FindElements(by);
    }
}
