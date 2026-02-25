using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWebdriverTask.CoreLayer.Utils;
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

    public string Url => WebDriver.Url;

    public WebDriverWait Wait { get; private set; }

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
        return new WebElementWrapper(this, Waiter.WaitForElements(by, () => WebDriver.FindElement(by), Wait));
    }

    public ReadOnlyCollection<WebElementWrapper> FindElements(By by)
    {
        var elements = new ReadOnlyCollection<IWebElement>([]);
        Waiter.WaitForElements(
            by,
            () =>
            {
                elements = WebDriver.FindElements(by);
                if (elements.Count == 0)
                {
                    return null;
                }

                return WrapElements(elements);
            },
            Wait);

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
