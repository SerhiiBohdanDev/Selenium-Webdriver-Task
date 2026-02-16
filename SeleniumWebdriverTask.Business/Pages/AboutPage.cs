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
        _driver.ScrollToElement(downloadButton);
        _driver.SafeClick(downloadButton);

        return this;
    }
}
