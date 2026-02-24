using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.CoreLayer.WebElement;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

public class SearchJobsPage
{
    private readonly DriverWrapper _driver;

    private readonly By _keywordSearchField = By.XPath("//*[@id='anchor-list']//form//input[@data-testid='search-input']");
    private readonly By _locationDropdown = By.XPath("//*[@id='anchor-list']//form//input[contains(@class, 'dropdown__input')]");
    private readonly By _remoteCheckbox = By.Id("checkbox-vacancy_type-Remote-«r0»");
    private readonly By _searchButton = By.XPath("//*[@id='anchor-list']//child::button[@type='submit']");
    private readonly By _resultsContainerFull = By.ClassName("List_content__r2Dmt");
    private readonly By _resultsContainer = By.ClassName("List_list___59gh");
    private readonly By _jobCardTitle = By.CssSelector("span[data-testid='job-card-title']");
    private readonly By _shortJobDescription = By.CssSelector("div[data-testid='job-card-description']");
    private readonly By _descriptionSentences = By.CssSelector("div[data-testid='rich-text']");
    private readonly By _lastElement = By.XPath("./*[last()]");

    public SearchJobsPage(DriverWrapper driver)
    {
        _driver = driver;
    }

    public SearchJobsPage EnterLanguage(string language)
    {
        var element = _driver.FindElement(_keywordSearchField);
        element
            .WaitUntilEnabled()
            .EnterText(language);

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

        // the checkbox has opacity at 0 which makes it Displayed property false, and so clicking is not allowed
        // so we use js to click
        checkbox.JavascriptClick();
        return this;
    }

    public SearchJobsPage ClickSearch()
    {
        var search = _driver.FindElement(_searchButton);
        search
            .WaitUntilEnabled()
            .SafeClick();

        return this;
    }

    public List<string> GetJobInformation()
    {
        var results = new List<string>();
        var containerStatic = _driver.FindElement(_resultsContainerFull);

        // this element is deleted for a moment when search happens, using containerStatic seems to solve the problem
        var containerDynamic = containerStatic.FindElement(_resultsContainer);
        containerDynamic.WaitUntilDisplayed();
        var lastResult = containerDynamic.FindElement(_lastElement);
        var title = lastResult.FindElement(_jobCardTitle);
        results.Add(title.Text);

        var shortDescription = lastResult.FindElement(_shortJobDescription);
        results.Add(shortDescription.Text);

        var sentences = lastResult.FindElements(_descriptionSentences);
        for (int i = 0; i < sentences.Count; i++)
        {
            var text = sentences[i].TextContent;
            if (text != null)
            {
                results.Add(text);
            }
        }

        return results;
    }
}
