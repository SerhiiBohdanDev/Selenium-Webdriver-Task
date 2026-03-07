using System.Text;
using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.CoreLayer.WebElement;
using SeleniumWebdriverTask.CoreLayer.WebElement.Elements;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

/// <summary>
/// Class to store information related to Search jobs page.
/// </summary>
public class SearchJobsPage : BasePage
{
    private readonly By _keywordSearchFieldLocator = By.XPath("//*[@id='anchor-list']//form//input[@data-testid='search-input']");
    private readonly By _locationDropdownLocator = By.XPath("//*[@id='anchor-list']//form//input[contains(@class, 'dropdown__input')]");
    private readonly By _remoteCheckboxLocator = By.XPath("//*[@class='List_sideMenu__wGtLn']//input[@name='vacancy_type-Remote']");
    private readonly By _searchButtonLocator = By.XPath("//*[@id='anchor-list']//child::button[@type='submit']");
    private readonly By _resultsContainerLocator = By.ClassName("List_list___59gh");
    private readonly By _jobCardTitleLocator = By.CssSelector("span[data-testid='job-card-title']");
    private readonly By _shortJobDescriptionLocator = By.CssSelector("div[data-testid='job-card-description']");
    private readonly By _descriptionSentencesLocator = By.CssSelector("div[data-testid='rich-text']");
    private readonly By _lastElementLocator = By.XPath("./*[last()]");

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchJobsPage"/> class.
    /// </summary>
    /// <param name="driver">DriverWrapper instance.</param>
    public SearchJobsPage(DriverWrapper driver)
        : base(driver)
    {
    }

    /// <summary>
    /// Waits until page is stabilized after initial load.
    /// </summary>
    /// <returns>SearchJobsPage instance.</returns>
    public SearchJobsPage WaitUntilCountryDetected()
    {
        DriverWrapper.WaitForCondition(() =>
        {
            return
            DriverWrapper.Url.Contains("careers")
            && DriverWrapper.Url.Contains("country")
            && !DriverWrapper.Url.Contains("utm");
        });

        return this;
    }

    /// <summary>
    /// Enters language into the keyword search field.
    /// </summary>
    /// <param name="language">Language to search for.</param>
    /// <returns>SearchJobsPage instance.</returns>
    public SearchJobsPage EnterLanguage(string language)
    {
        var element = DriverWrapper.FindElement<TextInput>(_keywordSearchFieldLocator);
        element.WaitUntilEnabled();
        element.EnterText(language);
        element.PressEnter();

        return this;
    }

    /// <summary>
    /// Enters location into the location field.
    /// </summary>
    /// <param name="location">Location to restrict search to.</param>
    /// <returns>SearchJobsPage instance.</returns>
    public SearchJobsPage EnterLocation(string location)
    {
        var element = DriverWrapper.FindElement<TextInput>(_locationDropdownLocator);
        element.WaitUntilEnabled();
        element.EnterText(location);
        element.PressEnter();

        return this;
    }

    /// <summary>
    /// Checks the 'remote' option checkbox.
    /// </summary>
    /// <returns>SearchJobsPage instance.</returns>
    public SearchJobsPage ClickRemoteCheckbox()
    {
        var checkbox = DriverWrapper.FindElement<Checkbox>(_remoteCheckboxLocator);
        checkbox.ScrollToElement();
        checkbox.Hover();

        // the checkbox has opacity at 0 which makes it Displayed property false, and so clicking is not allowed
        // so we use js to click
        checkbox.JavascriptClick();
        return this;
    }

    /// <summary>
    /// Clicks search button.
    /// </summary>
    /// <returns>SearchJobsPage instance.</returns>
    public SearchJobsPage ClickSearch()
    {
        var search = DriverWrapper.FindElement<Button>(_searchButtonLocator);
        search.ScrollToElement();
        search.WaitUntilEnabled();
        search.SafeClick();

        return this;
    }

    /// <summary>
    /// Gets the information abut the last job in the list.
    /// </summary>
    /// <returns>Full description of the job.</returns>
    public string GetJobInformation()
    {
        /* Have to wait for this element because
         * Firefox in headless mode throws StaleElement exception when looking for resultsContainer.
         */
        var search = DriverWrapper.FindElement<Button>(_searchButtonLocator);
        search.WaitUntilEnabled();

        var resultsContainer = DriverWrapper.FindElement<WebElementWrapper>(_resultsContainerLocator);
        resultsContainer.WaitUntilDisplayed();
        resultsContainer.ScrollToElement();

        var lastResult = resultsContainer.FindElement<WebElementWrapper>(_lastElementLocator);
        lastResult.WaitUntilDisplayed();

        var jobDescriptionSentences = new List<string>();
        var title = lastResult.FindElement<TextElement>(_jobCardTitleLocator);
        jobDescriptionSentences.Add(title.Text);
        var shortDescription = lastResult.FindElement<TextElement>(_shortJobDescriptionLocator);
        jobDescriptionSentences.Add(shortDescription.Text);

        var sentences = lastResult.FindElements<TextElement>(_descriptionSentencesLocator);
        for (int i = 0; i < sentences.Count; i++)
        {
            var text = sentences[i].TextContent;
            if (text != null)
            {
                jobDescriptionSentences.Add(text);
            }
        }

        var builder = new StringBuilder();
        for (int i = 0; i < jobDescriptionSentences.Count; i++)
        {
            builder.AppendLine(jobDescriptionSentences[i]);
        }

        return builder.ToString();
    }
}
