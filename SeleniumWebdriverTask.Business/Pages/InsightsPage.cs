using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.CoreLayer.WebElement;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

public class InsightsPage
{
    private readonly DriverWrapper _driver;
    private readonly By _nextSlideButton = By.XPath("//*[@class='slider-ui-23   media-content ']//button[@class='slider__right-arrow slider-navigation-arrow']");
    private readonly By _slideTitle = By.XPath("//*[@class='slider-ui-23   media-content ']//*[contains(@class,'active')]//span[@class='font-size-60']");
    private readonly By _readMoreLink = By.XPath("//*[@class='slider-ui-23   media-content ']//*[contains(@class,'active')]//a");

    public InsightsPage(DriverWrapper driver)
    {
        _driver = driver;
    }

    public InsightsPage ClickNextSlide()
    {
        var button = new WebElementWrapper(_driver, _driver.FindElement(_nextSlideButton));
        button
            .WaitUntilEnabled()
            .JavascriptClick();

        return this;
    }

    public string GetActiveSlideTitle()
    {
        var sentence = _driver.FindElement(_slideTitle);
        return DriverWrapper.GetElementText(sentence) ?? string.Empty;
    }

    public InsightsPage ClickMoreInfo()
    {
        var nextSlideButton = new WebElementWrapper(_driver, _driver.FindElement(_readMoreLink));
        nextSlideButton
            .WaitUntilEnabled()
            .JavascriptClick();

        return this;
    }
}
