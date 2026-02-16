using LocatorsForWebElements.CoreLayer;
using OpenQA.Selenium;

namespace LocatorsForWebElements.BusinessLayer.Pages;

public class AboutPage
{
    private readonly DriverWrapper _driver;
    private readonly By _downloadButton = By.XPath("//a[@download='']");
    //private readonly By _downloadButton = By.XPath("/html/body/div/div[2]/main/div[1]/div[5]/section/div[2]/div/div/div[1]/div/div[3]/div/a");

    public AboutPage(DriverWrapper driver)
    {
        _driver = driver;
    }

    public AboutPage ScrollToAndClickDownload()
    {
        var downloadButton = _driver.FindClickableElement(_downloadButton);
        _driver.WaitUntilLinkIsReady(downloadButton);
        _driver.ScrollToElement(downloadButton);
        _driver.SafeClick(downloadButton);

        return this;
    }
}
