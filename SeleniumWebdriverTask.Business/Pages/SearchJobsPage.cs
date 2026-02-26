using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

public class SearchJobsPage
{
    private readonly DriverWrapper _driver;

    private readonly By _keywordSearchField = By.XPath("//*[@id='anchor-list']//form//input[@data-testid='search-input']");
    private readonly By _locationDropdown = By.XPath("//*[@id='anchor-list']//form//input[contains(@class, 'dropdown__input')]");
    private readonly By _remoteCheckbox = By.XPath("//*[@class='List_sideMenu__wGtLn']//input[@name='vacancy_type-Remote']");
    private readonly By _searchButton = By.XPath("//*[@id='anchor-list']//child::button[@type='submit']");
    private readonly By _resultsContainer = By.ClassName("List_list___59gh");
    private readonly By _jobCardTitle = By.CssSelector("span[data-testid='job-card-title']");
    private readonly By _shortJobDescription = By.CssSelector("div[data-testid='job-card-description']");
    private readonly By _descriptionSentences = By.CssSelector("div[data-testid='rich-text']");
    private readonly By _lastElement = By.XPath("./*[last()]");

    public SearchJobsPage(DriverWrapper driver)
    {
        _driver = driver;
    }

    public SearchJobsPage WaitUntilCountryDetected()
    {
        _driver.WaitForCondition(() =>
        {
            return
            _driver.Url.Contains("careers")
            && _driver.Url.Contains("country")
            && !_driver.Url.Contains("utm");
        });

        return this;
    }

    public SearchJobsPage EnterLanguage(string language)
    {
        var element = _driver.FindElement(_keywordSearchField);
        element
            .WaitUntilEnabled()
            .EnterText(language);
        element.PressEnter();

        return this;
    }

    public SearchJobsPage EnterLocation(string location)
    {
        var element = _driver.FindElement(_locationDropdown);
        element
            .WaitUntilEnabled()
            .EnterText(location);
        element.PressEnter();

        return this;
    }

    public SearchJobsPage ClickRemoteCheckbox()
    {
        var checkbox = _driver.FindElement(_remoteCheckbox);
        checkbox.ScrollToElement();
        checkbox.Hover();

        // the checkbox has opacity at 0 which makes it Displayed property false, and so clicking is not allowed
        // so we use js to click
        checkbox.JavascriptClick();
        return this;
    }

    public SearchJobsPage ClickSearch()
    {
        var search = _driver.FindElement(_searchButton);
        search.ScrollToElement();
        search
            .WaitUntilEnabled()
            .SafeClick();

        return this;
    }

    public List<string> GetJobInformation()
    {
        /* Have to wait for this element because
         * Firefox in headless mode throws StaleElement exception when looking for resultsContainer.
         */
        var search = _driver.FindElement(_searchButton);
        search.WaitUntilEnabled();

        var resultsContainer = _driver.FindElement(_resultsContainer);
        resultsContainer.WaitUntilDisplayed();
        resultsContainer.ScrollToElement();

        var lastResult = resultsContainer.FindElement(_lastElement);
        lastResult.WaitUntilDisplayed();

        var jobDescriptionSentences = new List<string>();
        var title = lastResult.FindElement(_jobCardTitle);
        jobDescriptionSentences.Add(title.Text);
        var shortDescription = lastResult.FindElement(_shortJobDescription);
        jobDescriptionSentences.Add(shortDescription.Text);

        var sentences = lastResult.FindElements(_descriptionSentences);
        for (int i = 0; i < sentences.Count; i++)
        {
            var text = sentences[i].TextContent;
            if (text != null)
            {
                jobDescriptionSentences.Add(text);
            }
        }

        return jobDescriptionSentences;
    }
}
