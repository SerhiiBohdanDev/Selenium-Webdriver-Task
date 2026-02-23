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
    private readonly TimeSpan _timeout;

    public DriverWrapper(IWebDriver driver, TimeSpan timeout)
    {
        WebDriver = driver;
        _timeout = timeout;
    }

    public System.Drawing.Size WindowSize => WebDriver.Manage().Window.Size;

    public IWebDriver WebDriver { get; private set; }

    public static string? GetElementText(IWebElement element) => element.GetText();

    public static async Task<bool> WaitForFileToFinishChangingContentAsync(string filePath, string fileName, int pollingIntervalInSeconds, CancellationToken cancellationToken)
    {
        var fullFilePath = Path.Combine(filePath, fileName);
        await WaitForFileToExistAsync(fullFilePath, pollingIntervalInSeconds, cancellationToken);

        var fileSize = new FileInfo(fullFilePath).Length;
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }

            await Task.Delay(TimeSpan.FromSeconds(pollingIntervalInSeconds), cancellationToken);

            var newFileSize = new FileInfo(fullFilePath).Length;
            if (newFileSize == fileSize)
            {
                // Firefox does not let go of part files in time which prevents directory from being deleted.
                var partFile = Directory.GetFiles(filePath, "*.part").FirstOrDefault();
                if (partFile == null)
                {
                    return true;
                }
            }

            fileSize = newFileSize;
        }
    }

    public void GoToUrl(string url) => WebDriver.Navigate().GoToUrl(url);

    public void Close()
    {
        WebDriver.Quit();
        WebDriver.Dispose();
    }

    /// <summary>
    /// Allows clicking an element safely in cases where it can be interrupted by animation or popups.
    /// </summary>
    /// <param name="element">Element we're trying to click.</param>
    public void SafeClick(IWebElement element)
    {
        new Actions(WebDriver)
            .MoveToElement(element)
            .Click()
            .Build()
            .Perform();
    }

    public void JavascriptClick(IWebElement element)
    {
        WebDriver.ExecuteJavaScript(JavascriptClickCommand, element);
    }

    public void Hover(IWebElement element)
    {
        new Actions(WebDriver)
                .MoveToElement(element)
                .Perform();
    }

    public void ScrollToElement(IWebElement element)
    {
        WebDriver.ExecuteJavaScript(JavascriptScrollCommand, element);
    }

    public IWebElement FindElement(By by, IWebElement? parent = default)
    {
        return WaitForOneElement(by, parent);
    }

    public ReadOnlyCollection<IWebElement> FindElements(By by, IWebElement? parent = default)
    {
        return WaitForManyElements(by, parent);
    }

    public IWebElement FindDisplayedElement(By by, IWebElement? parent = default)
    {
        return WaitForOneElement(by, parent, IsElementDisplayed);
    }

    public IWebElement FindClickableElement(By by, IWebElement? parent = default)
    {
        return WaitForOneElement(by, parent, IsElementClickable);
    }

    public ReadOnlyCollection<IWebElement> FindClickableElements(By by, IWebElement? parent = default)
    {
        return WaitForManyElements(by, parent, IsElementClickable);
    }

    public IWebElement FindClickableLinkElement(By by, IWebElement? parent = default)
    {
        return WaitForOneElement(by, parent, IsLinkReady);
    }

    public void Maximize(bool headless)
    {
        if (headless)
        {
            WebDriver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
        }
        else
        {
            WebDriver.Manage().Window.Maximize();
        }
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
            return conditionMet ? element : null;
#pragma warning restore CS8603 // Possible null reference return.
        }
        else
        {
            return element;
        }
    }

    private static ReadOnlyCollection<IWebElement> FindManyElementsAndCheckCondition(Func<ReadOnlyCollection<IWebElement>> findAction, Func<IWebElement, bool>? conditionCheckAction = null)
    {
        var elements = findAction.Invoke();
        if (elements.Count == 0)
        {
#pragma warning disable S1168 // Possible null reference return.
#pragma warning disable CS8603 // Possible null reference return.
            return null;
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
            return conditionMet ? elements : null;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore S1168 // Possible null reference return.
        }
        else
        {
            return elements;
        }
    }

    private static IWebElement OneElementNotFound(By by, Type exceptionCaught)
    {
        if (exceptionCaught == typeof(NoSuchElementException))
        {
            throw new NoSuchElementException($"Could not find element located by {by} after {MaxRetries} attempts.");
        }
        else
        {
            throw new StaleElementReferenceException($"Element located by {by} remained stale after {MaxRetries} attempts.");
        }
    }

    private static ReadOnlyCollection<IWebElement> ManyElementsNotFound(By by, Type exceptionCaught)
    {
        if (exceptionCaught == typeof(NoSuchElementException))
        {
            return new ReadOnlyCollection<IWebElement>([]);
        }
        else
        {
            throw new StaleElementReferenceException($"Elements collection located by {by} remained stale after {MaxRetries} attempts.");
        }
    }

    /// <summary>
    /// A wrapper method to allow finding one IWebElement or ReadOnlyCollection of type IWebElement.
    /// </summary>
    /// <typeparam name="T">IWebElement or ReadOnlyCollection of type IWebElement.</typeparam>
    /// <param name="by">Locator to reference in exception.</param>
    /// <param name="findAndCheckElementAction">Action to find elements and check if they fit the required condition.</param>
    /// <returns>One IWebElement or ReadOnlyCollection(IWebElement).</returns>
    /// <exception cref="NoSuchElementException">Thrown if element was not found when looking for a single element.</exception>
    /// <exception cref="StaleElementReferenceException">Thrown if an element or collection were stale.</exception>
    private T WaitForOneOrManyElements<T>(By by, Func<T> findAndCheckElementAction, Func<By, Type, T> elementNotFoundAction)
    {
        int retries = 0;
        var exceptionCaught = typeof(NoSuchElementException);
        while (retries < MaxRetries)
        {
            try
            {
                var wait = new WebDriverWait(WebDriver, _timeout);
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

        return elementNotFoundAction.Invoke(by, exceptionCaught);
    }

    private IWebElement WaitForOneElement(By by, IWebElement? parent = default, Func<IWebElement, bool>? conditionCheckAction = null)
    {
        return WaitForOneOrManyElements(
        by,
        () =>
        {
            return FindElementAndCheckCondition(() => FindOneElement(by, parent), conditionCheckAction);
        },
        OneElementNotFound);
    }

    private ReadOnlyCollection<IWebElement> WaitForManyElements(By by, IWebElement? parent = default, Func<IWebElement, bool>? conditionCheckAction = null)
    {
        return WaitForOneOrManyElements(
        by,
        () =>
        {
            return FindManyElementsAndCheckCondition(() => FindManyElements(by, parent), conditionCheckAction);
        },
        ManyElementsNotFound);
    }

    private IWebElement FindOneElement(By by, IWebElement? parent = default)
    {
        return parent == null ? WebDriver.FindElement(by) : parent.FindElement(by);
    }

    private ReadOnlyCollection<IWebElement> FindManyElements(By by, IWebElement? parent = default)
    {
        return parent == null ? WebDriver.FindElements(by) : parent.FindElements(by);
    }
}
