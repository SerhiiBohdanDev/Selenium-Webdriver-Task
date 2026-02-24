using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

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
        var button = _driver.FindElement(_nextSlideButton);
        button
            .WaitUntilEnabled()
            .JavascriptClick();

        return this;
    }

    public string GetActiveSlideTitle()
    {
        var sentence = _driver.FindElement(_slideTitle);
        return sentence.TextContent ?? string.Empty;
    }

    public InsightsPage ClickMoreInfo()
    {
        var nextSlideButton = _driver.FindElement(_readMoreLink);
        nextSlideButton
            .WaitUntilEnabled()
            .JavascriptClick();

        return this;
    }
}
