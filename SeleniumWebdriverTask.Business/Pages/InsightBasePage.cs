using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

/// <summary>
/// Class to store information related to an Insight page.
/// </summary>
public class InsightBasePage : BasePage
{
    private readonly By _title = By.XPath("//*[@class='ai-report-page']//*[@class='layout-box']");

    /// <summary>
    /// Initializes a new instance of the <see cref="InsightBasePage"/> class.
    /// </summary>
    /// <param name="driver">DriverWrapper instance.</param>
    public InsightBasePage(DriverWrapper driver)
        : base(driver)
    {
    }

    /// <summary>
    /// Gets the title of the article.
    /// </summary>
    /// <returns>Title of the article or empty string if title was null.</returns>
    public string GetTitle()
    {
        var title = DriverWrapper.FindElement(_title);
        title.WaitUntilDisplayed();
        return title.Text ?? string.Empty;
    }
}
