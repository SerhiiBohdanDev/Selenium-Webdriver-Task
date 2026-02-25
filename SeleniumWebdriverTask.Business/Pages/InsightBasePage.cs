using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

public class InsightBasePage
{
    private readonly DriverWrapper _driver;
    private readonly By _title = By.XPath("//*[@class='header_and_download']//span[@class='font-size-80-33']");

    public InsightBasePage(DriverWrapper driver)
    {
        _driver = driver;
    }

    public string GetTitle()
    {
        var sentence = _driver.FindElement(_title);
        sentence.WaitUntilDisplayed();
        return sentence.Text ?? string.Empty;
    }
}
