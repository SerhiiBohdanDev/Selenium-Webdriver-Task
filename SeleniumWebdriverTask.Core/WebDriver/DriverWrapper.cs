using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWebdriverTask.CoreLayer.WebElement;

namespace SeleniumWebdriverTask.CoreLayer.WebDriver;

public class DriverWrapper
{
    public const int MaxRetries = 3;

    public DriverWrapper(IWebDriver driver, TimeSpan timeout)
    {
        WebDriver = driver;
        Wait = new WebDriverWait(WebDriver, timeout);
    }

    public IWebDriver WebDriver { get; private set; }

    public WebDriverWait Wait { get; private set; }

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
                // So we need to wait until they are deleted.
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

    public WebElementWrapper FindElement(By by)
    {
        return new WebElementWrapper(this, WaitForElements(by, () => WebDriver.FindElement(by)));
    }

    public ReadOnlyCollection<WebElementWrapper> FindElements(By by)
    {
        var elements = new ReadOnlyCollection<IWebElement>([]);
        WaitForElements(by, () =>
        {
            elements = WebDriver.FindElements(by);
            if (elements.Count == 0)
            {
                return null;
            }

            return WrapElements(elements);
        });

        return WrapElements(elements);
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
        while (retries < MaxRetries)
        {
            try
            {
                return Wait.Until(driver =>
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
            throw new NoSuchElementException($"Could not find element located by {by} after {MaxRetries} attempts.");
        }
        else
        {
            throw new StaleElementReferenceException($"Element located by {by} remained stale after {MaxRetries} attempts.");
        }
    }

    private ReadOnlyCollection<WebElementWrapper> WrapElements(ReadOnlyCollection<IWebElement> elements)
    {
        var wrappedElements = new List<WebElementWrapper>();
        for (int i = 0; i < elements.Count; i++)
        {
            wrappedElements.Add(new WebElementWrapper(this, elements[i]));
        }

        return wrappedElements.AsReadOnly();
    }
}
