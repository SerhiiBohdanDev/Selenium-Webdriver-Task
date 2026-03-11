using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.CoreLayer.WebElement.Elements;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

/// <summary>
/// Class to store information related to About page.
/// </summary>
public class AiArticlePage : BasePage
{
    private readonly By _articleTitleLocator = By.XPath("//*[@class='column-control']//*[@class='rte-text-gradient']");

    // text on the page contains &nbsp character, so using contains to find
    private readonly By _relatedExpertiseSectionLocator = By.XPath($"//*[contains(text(), 'Our Related Expertise')]");

    /// <summary>
    /// Initializes a new instance of the <see cref="AiArticlePage"/> class.
    /// </summary>
    /// <param name="driver">DriverWrapper instance.</param>
    public AiArticlePage(DriverWrapper driver)
        : base(driver)
    {
    }

    /// <summary>
    /// Gets a value indicating whether or not Related Expertise section is displayed.
    /// </summary>
    public bool IsRelatedExpertiseSectionDisplayed =>
        DriverWrapper.FindElement<TextElement>(_relatedExpertiseSectionLocator).IsDisplayed;

    /// <summary>
    /// Finds the title of the article.
    /// </summary>
    /// <returns>The title of the article.</returns>
    public string? GetArticleTitle()
    {
        return DriverWrapper.FindElement<TextElement>(_articleTitleLocator).TextContent;
    }
}
