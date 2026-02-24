using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

public class AboutPage
{
    private readonly DriverWrapper _driver;
    private readonly By _downloadButton = By.XPath("//a[@download='']");

    public AboutPage(DriverWrapper driver)
    {
        _driver = driver;
    }

    public AboutPage ScrollToAndClickDownload()
    {
        var downloadButton = _driver.FindElement(_downloadButton);
        downloadButton
            .WaitUntilLinkIsReady()
            .ScrollToElement();
        downloadButton.JavascriptClick();

        return this;
    }
}
