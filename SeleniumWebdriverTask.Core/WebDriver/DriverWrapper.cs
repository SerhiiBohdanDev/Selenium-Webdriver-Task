using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebdriverTask.CoreLayer.WebDriver;

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
        return WaitForOneOrManyElements(by, () =>
        {
            return FindElementAndCheckCondition(() => FindOneElement(by, parent));
        });
    }

    public ReadOnlyCollection<IWebElement> FindElements(By by, IWebElement? parent = default)
    {
        return WaitForOneOrManyElements(by, () =>
        {
            return FindElementsCollectionAndCheckCondition(() => FindManyElements(by, parent));
        });
    }

    public IWebElement FindDisplayedElement(By by, IWebElement? parent = default)
    {
        return WaitForOneOrManyElements(by, () =>
        {
            return FindElementAndCheckCondition(() => FindOneElement(by, parent), IsElementDisplayed);
        });
    }

    public IWebElement FindClickableElement(By by, IWebElement? parent = default)
    {
        return WaitForOneOrManyElements(by, () =>
        {
            return FindElementAndCheckCondition(() => FindOneElement(by, parent), IsElementClickable);
        });
    }

    public ReadOnlyCollection<IWebElement> FindClickableElements(By by, IWebElement? parent = default)
    {
        return WaitForOneOrManyElements(by, () =>
        {
            return FindElementsCollectionAndCheckCondition(() => FindManyElements(by, parent), IsElementClickable);
        });
    }

    public IWebElement FindClickableLinkElement(By by, IWebElement? parent = default)
    {
        return WaitForOneOrManyElements(by, () =>
        {
            return FindElementAndCheckCondition(() => FindOneElement(by, parent), IsLinkReady);
        });
    }

    public void Maximize() => _driver.Manage().Window.Maximize();

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

    private static bool IsElementDisplayed(IWebElement element) => element.Displayed;

    private static bool IsElementClickable(IWebElement element) => element.Displayed && element.Enabled;

    private static bool IsLinkReady(IWebElement element) => element.Displayed && element.Enabled && element.GetUrl() != null;

    private static IWebElement FindElementAndCheckCondition(Func<IWebElement> findAction, Func<IWebElement, bool>? conditionCheckAction = null)
    {
        var element = findAction.Invoke();
        if (conditionCheckAction != null)
        {
            bool conditionMet = conditionCheckAction.Invoke(element);
#pragma warning disable CS8603 // Possible null reference return.
            return conditionMet ? element : default;
#pragma warning restore CS8603 // Possible null reference return.
        }
        else
        {
            return element;
        }
    }

    private static ReadOnlyCollection<IWebElement> FindElementsCollectionAndCheckCondition(Func<ReadOnlyCollection<IWebElement>> findAction, Func<IWebElement, bool>? conditionCheckAction = null)
    {
        var elements = findAction.Invoke();
        if (elements.Count == 0)
        {
#pragma warning disable S1168 // Possible null reference return.
#pragma warning disable CS8603 // Possible null reference return.
            return default;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore S1168 // Possible null reference return.
        }

        if (conditionCheckAction != null)
        {
            bool conditionMet = true;
            for (int i = 0; i < elements.Count; i++)
            {
                if (!conditionCheckAction.Invoke(elements[i]))
                {
                    conditionMet = false;
                    break;
                }
            }
#pragma warning disable S1168 // Possible null reference return.
#pragma warning disable CS8603 // Possible null reference return.
            return conditionMet ? elements : default;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore S1168 // Possible null reference return.
        }
        else
        {
            return elements;
        }
    }

    /// <summary>
    /// A wrapper method to allow finding one IWebElement or ReadOnlyCollection of type IWebElement.
    /// </summary>
    /// <typeparam name="T">IWebElement or ReadOnlyCollection of type IWebElement.</typeparam>
    /// <param name="by">Locator to reference in exception.</param>
    /// <param name="findAndCheckElementAction">Action to find elements and check if they fit the required condition.</param>
    /// <returns>One IWebElement or ReadOnlyCollection of type IWebElement.</returns>
    /// <exception cref="ArgumentNullException">Thrown if locator is null.</exception>
    /// <exception cref="NoSuchElementException">Thrown if element was not found when looking for a single element.</exception>
    /// <exception cref="StaleElementReferenceException">Thrown if an element or collection were stale.</exception>
    private T WaitForOneOrManyElements<T>(By by, Func<T> findAndCheckElementAction)
    {
        ArgumentNullException.ThrowIfNull(by);

        int retries = 0;
        var exceptionCaught = typeof(NoSuchElementException);
        while (retries < MaxRetries)
        {
            try
            {
                var wait = new WebDriverWait(_driver, _timeout);
                return wait.Until(driver =>
                {
                    try
                    {
                        return findAndCheckElementAction.Invoke();
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
