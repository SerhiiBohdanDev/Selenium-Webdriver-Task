using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.CoreLayer.WebElement;
using SeleniumWebdriverTask.CoreLayer.WebElement.Elements;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

/// <summary>
/// Class to store information related to Main page.
/// </summary>
public class MainPage : BasePage
{
    private readonly By _topNavRowLocator = By.ClassName("top-navigation__row");
    private readonly By _careersTextLocator = By.LinkText("Careers");
    private readonly By _joinUsLocator = By.PartialLinkText("Join our Team");
    private readonly By _magnifyingGlassLocator = By.CssSelector("button[class='header-search__button header__icon']");
    private readonly By _searchFieldLocator = By.Name("q");
    private readonly By _findButtonLocator = By.XPath("//form//child::button");
    private readonly By _searchResultLocator = By.ClassName("search-results__title-link");
    private readonly By _aboutLocator = By.LinkText("About");
    private readonly By _insightsLocator = By.LinkText("Insights");
    private readonly By _corporateResponsibilityLocator = By.LinkText("Corporate Responsibility");
    private readonly By _servicesLocator = By.LinkText("Services");

    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class.
    /// </summary>
    /// <param name="driver">DriverWrapper instance.</param>
    public MainPage(DriverWrapper driver)
        : base(driver)
    {
    }

    /// <summary>
    /// Click 'Join us' link.
    /// </summary>
    /// <returns>MainPage instance.</returns>
    public MainPage ClickJoinUs()
    {
        var topRow = DriverWrapper.FindElement<WebElementWrapper>(_topNavRowLocator);
        var careers = topRow.FindElement<TextElement>(_careersTextLocator);
        careers.Hover();

        var joinUsLink = topRow.FindElement<Link>(_joinUsLocator);
        joinUsLink.WaitUntilEnabled();
        joinUsLink.SafeClick();

        return this;
    }

    /// <summary>
    /// Click 'About' link.
    /// </summary>
    /// <returns>MainPage instance.</returns>
    public MainPage ClickAbout()
    {
        var topRow = DriverWrapper.FindElement<WebElementWrapper>(_topNavRowLocator);
        var about = topRow.FindElement<Link>(_aboutLocator);
        about.WaitUntilEnabled();
        about.SafeClick();

        return this;
    }

    /// <summary>
    /// Click 'Insights' link.
    /// </summary>
    /// <returns>MainPage instance.</returns>
    public MainPage ClickInsights()
    {
        var topRow = DriverWrapper.FindElement<WebElementWrapper>(_topNavRowLocator);
        var insights = topRow.FindElement<Link>(_insightsLocator);
        insights.WaitUntilEnabled();
        insights.SafeClick();

        return this;
    }

    /// <summary>
    /// Click magnifying glass button.
    /// </summary>
    /// <returns>MainPage instance.</returns>
    public MainPage ClickMagnifyingGlass()
    {
        DriverWrapper
            .FindElement<Button>(_magnifyingGlassLocator)
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
        var searchField = DriverWrapper.FindElement<TextInput>(_searchFieldLocator);
        searchField.WaitUntilEnabled();
        searchField.EnterText(text);

        return this;
    }

    /// <summary>
    /// Click 'Find' button.
    /// </summary>
    /// <returns>MainPage instance.</returns>
    public MainPage ClickFind()
    {
        var findButton = DriverWrapper.FindElement<Button>(_findButtonLocator);
        findButton.WaitUntilEnabled();
        findButton.SafeClick();

        return this;
    }

    /// <summary>
    /// Click 'Corporate responsibility' link.
    /// </summary>
    /// <returns>MainPage instance.</returns>
    public MainPage ClickCorporateResponsibility()
    {
        var topRow = DriverWrapper.FindElement<WebElementWrapper>(_topNavRowLocator);
        var about = topRow.FindElement<Link>(_aboutLocator);
        about.Hover();
        topRow
            .FindElement<Link>(_corporateResponsibilityLocator)
            .SafeClick();

        return this;
    }

    /// <summary>
    /// Clicks a link to article about AI.
    /// </summary>
    /// <param name="articleName">Name of the article to find link by.</param>
    public void ClickAiArticleLink(string articleName)
    {
        var topRow = DriverWrapper.FindElement<WebElementWrapper>(_topNavRowLocator);
        var servicesLink = topRow.FindElement<Link>(_servicesLocator);
        servicesLink.Hover();
        topRow
            .FindElement<Link>(By.LinkText(articleName))
            .SafeClick();
    }

    /// <summary>
    /// Gets the titles of search results.
    /// </summary>
    /// <returns>The titles of search results.</returns>
    public List<string> GetSearchResultTitles()
    {
        var results = new List<string>();
        var elements = DriverWrapper.FindElements<TextElement>(_searchResultLocator);
        foreach (var element in elements)
        {
            results.Add(element.Text);
        }

        return results;
    }
}
