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
        var nextSlideButton = _driver.FindClickableElement(_nextSlideButton);
        _driver.JavascriptClick(nextSlideButton);

        return this;
    }

    public string GetActiveSlideTitle()
    {
        var sentence = _driver.FindElement(_slideTitle);
        return DriverWrapper.GetElementText(sentence) ?? string.Empty;
    }

    public InsightsPage ClickMoreInfo()
    {
        var nextSlideButton = _driver.FindClickableElement(_readMoreLink);
        _driver.JavascriptClick(nextSlideButton);

        return this;
    }
}
