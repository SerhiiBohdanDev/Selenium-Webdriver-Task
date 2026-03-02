using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

/// <summary>
/// Class to store information related to About page.
/// </summary>
public class AboutPage
{
    private readonly DriverWrapper _driver;
    private readonly By _downloadButton = By.XPath("//a[@download='']");

    /// <summary>
    /// Initializes a new instance of the <see cref="AboutPage"/> class.
    /// </summary>
    /// <param name="driver">DriverWrapper instance.</param>
    public AboutPage(DriverWrapper driver)
    {
        _driver = driver;
    }

    /// <summary>
    /// Scrolls page to download button.
    /// </summary>
    /// <returns>AboutPage instance.</returns>
    public AboutPage ScrollToDownloadButton()
    {
        var downloadButton = _driver.FindElement(_downloadButton);
        downloadButton
            .WaitUntilLinkIsReady()
            .ScrollToElement();

        return this;
    }

    /// <summary>
    /// Clicks download button.
    /// </summary>
    /// <returns>AboutPage instance.</returns>
    public AboutPage ClickDownloadButton()
    {
        var downloadButton = _driver.FindElement(_downloadButton);
        downloadButton
            .WaitUntilLinkIsReady()
            .JavascriptClick();

        return this;
    }
}
