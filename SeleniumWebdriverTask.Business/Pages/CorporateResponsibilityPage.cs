using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

/// <summary>
/// Class to store information related to Corporate Responsibility page.
/// </summary>
public class CorporateResponsibilityPage
{
    private readonly DriverWrapper _driver;
    private readonly By _downloadLink = By.LinkText("Download Our ESG Quick Facts");

    /// <summary>
    /// Initializes a new instance of the <see cref="CorporateResponsibilityPage"/> class.
    /// </summary>
    /// <param name="driver">DriverWrapper instance.</param>
    public CorporateResponsibilityPage(DriverWrapper driver)
    {
        _driver = driver;
    }

    /// <summary>
    /// Scrolls page to download link.
    /// </summary>
    /// <returns>Instance of CorporateResponsibilityPage.</returns>
    public CorporateResponsibilityPage ScrollToDownloadLink()
    {
        var downloadButton = _driver.FindElement(_downloadLink);
        downloadButton
            .WaitUntilLinkIsReady()
            .ScrollToElement();

        return this;
    }

    /// <summary>
    /// Clicks download link.
    /// </summary>
    /// <returns>Instance of CorporateResponsibilityPage.</returns>
    public CorporateResponsibilityPage ClickDownloadLink()
    {
        var downloadButton = _driver.FindElement(_downloadLink);
        downloadButton
            .WaitUntilLinkIsReady()
            .JavascriptClick();

        return this;
    }
}
