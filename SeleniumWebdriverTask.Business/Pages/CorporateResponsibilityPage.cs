using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.CoreLayer.WebElement.Elements;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

/// <summary>
/// Class to store information related to Corporate Responsibility page.
/// </summary>
public class CorporateResponsibilityPage : BasePage
{
    private readonly By _downloadLink = By.LinkText("Download Our ESG Quick Facts");

    /// <summary>
    /// Initializes a new instance of the <see cref="CorporateResponsibilityPage"/> class.
    /// </summary>
    /// <param name="driver">DriverWrapper instance.</param>
    public CorporateResponsibilityPage(DriverWrapper driver)
        : base(driver)
    {
    }

    /// <summary>
    /// Scrolls page to download link.
    /// </summary>
    /// <returns>Instance of CorporateResponsibilityPage.</returns>
    public CorporateResponsibilityPage ScrollToDownloadLink()
    {
        var downloadLink = DriverWrapper.FindElement<Link>(_downloadLink);
        downloadLink
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
        var downloadLink = DriverWrapper.FindElement<Link>(_downloadLink);
        downloadLink
            .WaitUntilLinkIsReady()
            .JavascriptClick();

        return this;
    }
}
