using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

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
        var topRow = _driver.FindElement(_topNavRow);
        var careers = _driver.FindClickableElement(_careersText, topRow);
        _driver.Hover(careers);

        _driver
            .FindClickableElement(_joinUs, topRow)
            .Click();

        return this;
    }

    public MainPage ClickAbout()
    {
        var topRow = _driver.FindElement(_topNavRow);
        _driver
            .FindClickableElement(_about, topRow)
            .Click();

        return this;
    }

    public MainPage ClickInsights()
    {
        var topRow = _driver.FindElement(_topNavRow);
        _driver
            .FindClickableElement(_insights, topRow)
            .Click();

        return this;
    }

    public MainPage ClickMagnifyingGlass()
    {
        _driver
            .FindClickableElement(_magnifyingGlass)
            .Click();
        return this;
    }

    public MainPage EnterSearchTerm(string text)
    {
        _driver
            .FindClickableElement(_searchField)
            .SendKeys(text);
        return this;
    }

    public MainPage ClickFind()
    {
        _driver
            .FindClickableElement(_findButton)
            .Click();

        return this;
    }

    public MainPage ClickCorporateResponsibility()
    {
        var topRow = _driver.FindElement(_topNavRow);
        _driver.Hover(_driver.FindClickableElement(_about, topRow));
        _driver.FindClickableElement(_corporateResponsibility, topRow)
            .Click();

        return this;
    }

    public List<string> GetSearchResultTitles()
    {
        var results = new List<string>();
        var elements = _driver.FindClickableElements(_searchResult);
        foreach (var element in elements)
        {
            results.Add(element.Text);
        }

        return results;
    }
}
