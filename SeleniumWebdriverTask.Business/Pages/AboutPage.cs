using LocatorsForWebElements.CoreLayer;
using OpenQA.Selenium;

namespace LocatorsForWebElements.BusinessLayer.Pages;

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
        var downloadButton = _driver.FindClickableElement(_downloadButton);
        _driver.WaitUntilLinkIsReady(downloadButton);
        _driver.ScrollToElement(downloadButton);

        // without accessing these properties we're redirected to the main page when clicking button
        var _ = downloadButton.Enabled && downloadButton.Displayed;
        _driver.JavascriptClick(downloadButton);

        return this;
    }
}
