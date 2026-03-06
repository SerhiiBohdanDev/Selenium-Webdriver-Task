using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWebdriverTask.CoreLayer.Utils;
using SeleniumWebdriverTask.CoreLayer.WebElement;

namespace SeleniumWebdriverTask.CoreLayer.WebDriver;

/// <summary>
/// A wrapper class for IWebDriver.
/// </summary>
public class DriverWrapper
{
    private System.Drawing.Size _headlessWindowSize = new System.Drawing.Size(1920, 1080);

    /// <summary>
    /// Initializes a new instance of the <see cref="DriverWrapper"/> class.
    /// </summary>
    /// <param name="driver">Instance of a IWebDriver.</param>
    /// <param name="timeout">Default timeout for WebDriverWait.</param>
    public DriverWrapper(IWebDriver driver, TimeSpan timeout)
    {
        WebDriver = driver;
        Wait = new WebDriverWait(WebDriver, timeout);
    }

    /// <summary>
    /// Gets IWebDriver instance.
    /// </summary>
    public IWebDriver WebDriver { get; private set; }

    /// <summary>
    /// Gets current page url.
    /// </summary>
    public string Url => WebDriver.Url;

    /// <summary>
    /// Gets WebDriverWait instance.
    /// </summary>
    public WebDriverWait Wait { get; private set; }

    /// <summary>
    /// Waits until file stops changing.
    /// </summary>
    /// <param name="filePath">Folder in which file is locafted.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="pollingIntervalInSeconds">How often to check for change in file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if file has finished changing.</returns>
    /// <exception cref="TaskCanceledException">Thrown if cancellation was called.</exception>
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

    /// <summary>
    /// Loads new page.
    /// </summary>
    /// <param name="url">Url of the new page.</param>
    public void GoToUrl(string url) => WebDriver.Navigate().GoToUrl(url);

    /// <summary>
    /// Closes Web driver and disposes of it.
    /// </summary>
    public void Close()
    {
        WebDriver.Quit();
        WebDriver.Dispose();
    }

    /// /// <summary>
    /// Finds and wraps an element.
    /// </summary>
    /// <typeparam name="T">WebElementWrapper type.</typeparam>
    /// <param name="by">Element locator.</param>
    /// <returns>Instance of WebElementWrapper.</returns>
    public T FindElement<T>(By by)
        where T : WebElementWrapper
    {
        var element = ElementsFinder.FindElement(by, WebDriver, Wait);
        return element.WrapElement<T>(this);
    }

    /// <summary>
    /// Finds and wraps collection of elements.
    /// </summary>
    /// <typeparam name="T">WebElementWrapper type.</typeparam>
    /// <param name="by">Element locator.</param>
    /// <returns>A collection of elements or empty collection if non were found.</returns>
    public ReadOnlyCollection<T> FindElements<T>(By by)
        where T : WebElementWrapper
    {
        var elements = ElementsFinder.FindElements(by, WebDriver, Wait);
        return elements.WrapElements<T>(this);
    }

    /// <summary>
    /// Maximizes browser window or sets it to a certain size in headless mode.
    /// </summary>
    /// <param name="headless">Is driver in headless mode.</param>
    public void Maximize(bool headless)
    {
        if (headless)
        {
            WebDriver.Manage().Window.Size = _headlessWindowSize;
        }
        else
        {
            WebDriver.Manage().Window.Maximize();
        }
    }

    /// <summary>
    /// Waits until condition becomes true or time runs out.
    /// </summary>
    /// <param name="condition">Condition to check.</param>
    public void WaitForCondition(Func<bool> condition)
    {
        Waiter.WaitForCondition(Wait, condition);
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
}
