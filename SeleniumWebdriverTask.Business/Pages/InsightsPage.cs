using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

/// <summary>
/// Class to store information related to Insights page.
/// </summary>
public class InsightsPage : BasePage
{
    private readonly By _nextSlideButton = By.XPath("//*[@class='slider-ui-23   media-content ']//button[@class='slider__right-arrow slider-navigation-arrow']");
    private readonly By _slideTitle = By.XPath("//*[@class='slider-ui-23   media-content ']//*[contains(@class,'active')]//span[@class='font-size-60']");
    private readonly By _readMoreLink = By.XPath("//*[@class='slider-ui-23   media-content ']//*[contains(@class,'active')]//a");

    /// <summary>
    /// Initializes a new instance of the <see cref="InsightsPage"/> class.
    /// </summary>
    /// <param name="driver">DriverWrapper instance.</param>
    public InsightsPage(DriverWrapper driver)
        : base(driver)
    {
    }

    /// <summary>
    /// Waiting until url is switched to the insights page.
    /// </summary>
    /// <returns>InsightsPage instance.</returns>
    public InsightsPage WaitUntilPageSwitched()
    {
        // Need to wait because firefox is not switching to this page in time
        DriverWrapper.WaitForCondition(() => DriverWrapper.Url.Contains("insights"));
        return this;
    }

    /// <summary>
    /// Clicking button to show next slide.
    /// </summary>
    /// <returns>InsightsPage instance.</returns>
    public InsightsPage ClickNextSlide()
    {
        var button = DriverWrapper.FindElement(_nextSlideButton);
        button.WaitUntilEnabled();
        var slideTitle = DriverWrapper.FindElement(_slideTitle);
        slideTitle.WaitUntilDisplayed();
        button.JavascriptClick();

        return this;
    }

    /// <summary>
    /// Gets the title of the currently active slide.
    /// </summary>
    /// <returns>The title of the currently active slide.</returns>
    public string GetActiveSlideTitle()
    {
        var slideTitle = DriverWrapper.FindElement(_slideTitle);
        slideTitle.WaitUntilDisplayed();
        return slideTitle.TextContent ?? string.Empty;
    }

    /// <summary>
    /// Click 'More info' button.
    /// </summary>
    /// <returns>InsightsPage instance.</returns>
    public InsightsPage ClickMoreInfo()
    {
        var readMoreButton = DriverWrapper.FindElement(_readMoreLink);
        readMoreButton.WaitUntilEnabled();
        readMoreButton.JavascriptClick();

        return this;
    }
}
