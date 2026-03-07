using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.CoreLayer.WebElement.Elements;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

/// <summary>
/// Class to store information related to About page.
/// </summary>
public class AboutPage : BasePage
{
    private readonly By _downloadButtonLocator = By.XPath("//a[@download='']");

    /// <summary>
    /// Initializes a new instance of the <see cref="AboutPage"/> class.
    /// </summary>
    /// <param name="driver">DriverWrapper instance.</param>
    public AboutPage(DriverWrapper driver)
        : base(driver)
    {
    }

    /// <summary>
    /// Scrolls page to download button.
    /// </summary>
    /// <returns>AboutPage instance.</returns>
    public AboutPage ScrollToDownloadButton()
    {
        var downloadLink = DriverWrapper.FindElement<Link>(_downloadButtonLocator);
        downloadLink
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
        var downloadLink = DriverWrapper.FindElement<Link>(_downloadButtonLocator);
        downloadLink
            .WaitUntilLinkIsReady()
            .JavascriptClick();

        return this;
    }
}
