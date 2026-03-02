using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

/// <summary>
/// Class to store information related to Main page.
/// </summary>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class.
    /// </summary>
    /// <param name="driver">DriverWrapper instance.</param>
    public MainPage(DriverWrapper driver)
    {
        _driver = driver;
    }

    /// <summary>
    /// Click 'Join us' link.
    /// </summary>
    /// <returns>MainPage instance.</returns>
    public MainPage ClickJoinUs()
    {
        var topRow = _driver.FindElement(_topNavRow);
        var careers = topRow.FindElement(_careersText);
        careers.Hover();

        var joinUsLink = topRow.FindElement(_joinUs);
        joinUsLink
            .WaitUntilEnabled()
            .SafeClick();

        return this;
    }

    /// <summary>
    /// Click 'About' link.
    /// </summary>
    /// <returns>MainPage instance.</returns>
    public MainPage ClickAbout()
    {
        var topRow = _driver.FindElement(_topNavRow);
        var about = topRow.FindElement(_about);
        about
            .WaitUntilEnabled()
            .SafeClick();

        return this;
    }

    /// <summary>
    /// Click 'Insights' link.
    /// </summary>
    /// <returns>MainPage instance.</returns>
    public MainPage ClickInsights()
    {
        var topRow = _driver.FindElement(_topNavRow);
        var insights = topRow.FindElement(_insights);
        insights
            .WaitUntilEnabled()
            .SafeClick();

        return this;
    }

    /// <summary>
    /// Click magnifying glass button.
    /// </summary>
    /// <returns>MainPage instance.</returns>
    public MainPage ClickMagnifyingGlass()
    {
        _driver
            .FindElement(_magnifyingGlass)
            .SafeClick();

        return this;
    }

    /// <summary>
    /// Enter text into search field.
    /// </summary>
    /// <param name="text">Text to search for.</param>
    /// <returns>MainPage instance.</returns>
    public MainPage EnterSearchTerm(string text)
    {
        var searchField = _driver.FindElement(_searchField);
        searchField
            .WaitUntilEnabled()
            .EnterText(text);

        return this;
    }

    /// <summary>
    /// Click 'Find' button.
    /// </summary>
    /// <returns>MainPage instance.</returns>
    public MainPage ClickFind()
    {
        var findButton = _driver.FindElement(_findButton);
        findButton
            .WaitUntilEnabled()
            .SafeClick();

        return this;
    }

    /// <summary>
    /// Click 'Corporate responsibility' link.
    /// </summary>
    /// <returns>MainPage instance.</returns>
    public MainPage ClickCorporateResponsibility()
    {
        var topRow = _driver.FindElement(_topNavRow);
        var about = topRow.FindElement(_about);
        about.Hover();
        topRow
            .FindElement(_corporateResponsibility)
            .SafeClick();

        return this;
    }

    /// <summary>
    /// Gets the titles of search results.
    /// </summary>
    /// <returns>The titles of search results.</returns>
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
