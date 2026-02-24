using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.CoreLayer.WebElement;

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
        var downloadButton = new WebElementWrapper(_driver, _driver.FindElement(_downloadButton));
        downloadButton
            .WaitUntilEnabled()
            .ScrollToElement();
        downloadButton.JavascriptClick();

        return this;
    }
}
