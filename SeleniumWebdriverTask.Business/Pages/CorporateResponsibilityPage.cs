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
        var downloadButton = _driver.FindElement(_downloadLink);
        downloadButton
            .WaitUntilEnabled()
            .ScrollToElement();
        downloadButton.JavascriptClick();

        return this;
    }
}
