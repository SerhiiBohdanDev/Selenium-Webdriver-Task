using LocatorsForWebElements.CoreLayer;
using OpenQA.Selenium;

namespace LocatorsForWebElements.BusinessLayer.Pages;

public class InsightBasePage
{
    private readonly DriverWrapper _driver;
    private readonly By _title = By.XPath("//*[@class='detail-page23__section detail-page23__section--top no-link']//span[@class='font-size-80-33']");

    public InsightBasePage(DriverWrapper driver)
    {
        _driver = driver;
    }

    public string GetTitle()
    {
        var sentence = _driver.FindElement(_title);
        return DriverWrapper.GetElementText(sentence) ?? string.Empty;
    }
}
