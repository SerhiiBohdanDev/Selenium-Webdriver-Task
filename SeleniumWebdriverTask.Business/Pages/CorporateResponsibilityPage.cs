using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

public class CorporateResponsibilityPage
{
    private readonly DriverWrapper _driver;
    private readonly By _downloadLink = By.LinkText("Download Our ESG Quick Facts");

    public CorporateResponsibilityPage(DriverWrapper driver)
    {
        _driver = driver;
    }

    public CorporateResponsibilityPage ScrollToAndClickDownload()
    {
        var downloadButton = _driver.FindClickableLinkElement(_downloadLink);
        _driver.ScrollToElement(downloadButton);
        _driver.JavascriptClick(downloadButton);

        return this;
    }
}
