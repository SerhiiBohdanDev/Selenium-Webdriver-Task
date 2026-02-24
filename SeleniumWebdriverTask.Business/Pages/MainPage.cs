using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.CoreLayer.WebElement;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

public class MainPage
{
    private readonly DriverWrapper _driver;

    private readonly By _topNavRow = By.ClassName("top-navigation__row");
    private readonly By _careersText = By.LinkText("Careers");
    private readonly By _joinUs = By.PartialLinkText("Join our Team");
    private readonly By _magnifyingGlass = By.CssSelector("button[class='header-search__button header__icon']");
    private readonly By _searchField = By.Name("q");
    private readonly By _findButton = By.XPath("//form//child::button");
    private readonly By _searchResult = By.ClassName("search-results__title-link");
    private readonly By _about = By.LinkText("About");
    private readonly By _insights = By.LinkText("Insights");
    private readonly By _corporateResponsibility = By.LinkText("Corporate Responsibility");

    public MainPage(DriverWrapper driver)
    {
        _driver = driver;
    }

    public MainPage ClickJoinUs()
    {
        var topRow = new WebElementWrapper(_driver, _driver.FindElement(_topNavRow));
        var careers = new WebElementWrapper(_driver, topRow.FindElement(_careersText));
        careers.Hover();

        var joinUsLink = new WebElementWrapper(_driver, topRow.FindElement(_joinUs));
        joinUsLink
            .WaitUntilEnabled()
            .SafeClick();

        return this;
    }

    public MainPage ClickAbout()
    {
        var topRow = new WebElementWrapper(_driver, _driver.FindElement(_topNavRow));
        var about = new WebElementWrapper(_driver, topRow.FindElement(_about));
        about
            .WaitUntilEnabled()
            .SafeClick();

        return this;
    }

    public MainPage ClickInsights()
    {
        var topRow = new WebElementWrapper(_driver, _driver.FindElement(_topNavRow));
        var insights = new WebElementWrapper(_driver, topRow.FindElement(_insights));
        insights
            .WaitUntilEnabled()
            .SafeClick();

        return this;
    }

    public MainPage ClickMagnifyingGlass()
    {
        new WebElementWrapper(_driver, _driver.FindElement(_magnifyingGlass))
            .SafeClick();

        return this;
    }

    public MainPage EnterSearchTerm(string text)
    {
        var searchField = new WebElementWrapper(_driver, _driver.FindElement(_searchField));
        searchField.EnterText(text);

        return this;
    }

    public MainPage ClickFind()
    {
        var findButton = new WebElementWrapper(_driver, _driver.FindElement(_findButton));
        findButton
            .WaitUntilEnabled()
            .SafeClick();

        return this;
    }

    public MainPage ClickCorporateResponsibility()
    {
        var topRow = new WebElementWrapper(_driver, _driver.FindElement(_topNavRow));
        var about = new WebElementWrapper(_driver, topRow.FindElement(_about));
        about.Hover();
        new WebElementWrapper(_driver, topRow.FindElement(_corporateResponsibility))
            .SafeClick();

        return this;
    }

    public List<string> GetSearchResultTitles()
    {
        var results = new List<string>();
        var elements = _driver.FindElements(_searchResult);
        foreach (var element in elements)
        {
            results.Add(element.Text);
        }

        return results;
    }
}
