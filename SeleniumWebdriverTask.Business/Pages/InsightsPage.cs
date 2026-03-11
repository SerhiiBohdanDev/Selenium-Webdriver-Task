using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.CoreLayer.WebElement.Elements;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

/// <summary>
/// Class to store information related to Insights page.
/// </summary>
public class InsightsPage : BasePage
{
    private readonly By _nextSlideButtonLocator = By.XPath("//*[contains(@class,'media-content')]//button[contains(@class,'slider__right-arrow')]");
    private readonly By _slideTitleLocator = By.XPath("//*[contains(@class,'media-content')]//*[contains(@class,'active')]//*[@class='text']");
    private readonly By _readMoreLinkLocator = By.XPath("//*[contains(@class,'media-content')]//*[contains(@class,'active')]//a");

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
        var button = DriverWrapper.FindElement<Button>(_nextSlideButtonLocator);
        button.WaitUntilEnabled();
        var slideTitle = DriverWrapper.FindElement<TextElement>(_slideTitleLocator);
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
        var slideTitle = DriverWrapper.FindElement<TextElement>(_slideTitleLocator);
        slideTitle.WaitUntilDisplayed();
        return slideTitle.TextContent ?? string.Empty;
    }

    /// <summary>
    /// Click 'More info' button.
    /// </summary>
    /// <returns>InsightsPage instance.</returns>
    public InsightsPage ClickMoreInfo()
    {
        var readMoreLink = DriverWrapper.FindElement<Link>(_readMoreLinkLocator);
        readMoreLink.WaitUntilEnabled();
        readMoreLink.JavascriptClick();

        return this;
    }
}
