// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Text;
using SeleniumWebdriverTask.BusinessLayer.Pages;
using TechTalk.SpecFlow;

namespace SeleniumWebdriverTask.TestLayer.Steps;

/// <summary>
/// Steps related to main page.
/// </summary>
[Binding]
internal class MainPageSteps : CommonSteps
{
    private const string TitleMissingSearchTermMessage = "Following titles are missing the following search term:";

    private readonly MainPage _mainPage;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainPageSteps"/> class.
    /// </summary>
    public MainPageSteps()
    {
        _mainPage = new MainPage(Driver);
    }

    /// <summary>
    /// Hovers over 'Services' link.
    /// </summary>
    [Given(@"I hover over 'Services' link")]
    public void HoverOverServicesLink()
    {
        _mainPage.HoverServicesLink();
    }

    /// <summary>
    /// Clicks on article link.
    /// </summary>
    /// <param name="articleName">Article name.</param>
    [When(@"I click on link to the '(.*)' article")]
    public void ClickOnLinkToTheArticle(string articleName)
    {
        _mainPage.ClickAiArticleLink(articleName);
    }

    /// <summary>
    /// Browser shows article page.
    /// </summary>
    /// <param name="articleName">Article name.</param>
    [Then(@"Browser navigates to the '(.*)' page")]
    public void BrowserNavigatesToArticlePage(string articleName)
    {
        var aiArticlePage = new AiArticlePage(Driver);
        var title = aiArticlePage.GetArticleTitle();
        Assert.That(title, Is.EqualTo(articleName));
    }

    /// <summary>
    /// Checks that 'Our Related Expertise' section is shown.
    /// </summary>
    [Then(@"'Our Related Expertise' section is displayed")]
    public void RelatedExpertiseSecionIsDisplayed()
    {
        var aiArticlePage = new AiArticlePage(Driver);
        var relatedExpertiseSectionDisplayed = aiArticlePage.IsRelatedExpertiseSectionDisplayed;
        Assert.That(relatedExpertiseSectionDisplayed, Is.True);
    }

    /// <summary>
    /// Clicks on insights link.
    /// </summary>
    [Given(@"I click 'Insights' link")]
    public void ClickOnInsightsLink()
    {
        _mainPage.ClickInsights();
    }

    /// <summary>
    /// Clicks on corporate responsibility link.
    /// </summary>
    [Given(@"I click 'Corporate responsibility' link")]
    public void ClickCorporateResponsibilityLink()
    {
        _mainPage.ClickCorporateResponsibility();
    }

    /// <summary>
    /// Clicks magnifying glass.
    /// </summary>
    [Given(@"I click on magnifying glass on main page")]
    public void ClickMagnifyingGlass()
    {
        _mainPage.ClickMagnifyingGlass();
    }

    /// <summary>
    /// Enters term and clicks search.
    /// </summary>
    /// <param name="term">Term to search for.</param>
    [When(@"I enter '(.*)' in the input field and click search")]
    public void EnterTermAndClickSearch(string term)
    {
        Logger.LogInformation($"Starting main page search, searching for '{term}'");
        _mainPage
            .EnterSearchTerm(term)
            .ClickFind();
    }

    /// <summary>
    /// Enters term and clicks search.
    /// </summary>
    /// <param name="term">Term to search for.</param>
    [Then(@"A list of results displayed containing '(.*)' in the title")]
    public void CheckSearchResults(string term)
    {
        var titles = _mainPage.GetSearchResultTitles();
        LogAllTitles(titles);

        var allTitlesContainTerm = true;
        List<string> titlesNotContainingTerm = [];
        foreach (var title in titles.Where(title => !title.Contains(term, StringComparison.InvariantCultureIgnoreCase)))
        {
            allTitlesContainTerm = false;
            titlesNotContainingTerm.Add(title);
        }

        LogTitlesNotContainingTerm(term, titlesNotContainingTerm);

        Assert.That(allTitlesContainTerm, Is.True);
    }

    /// <summary>
    /// Clicks join us link.
    /// </summary>
    [Given(@"I click join us link")]
    public void ClickJoinUs()
    {
        _mainPage.ClickJoinUs();
    }

    private void LogAllTitles(List<string> titles)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Found titles:");
        for (var i = 0; i < titles.Count; i++)
        {
            builder.AppendLine(titles[i]);
        }

        Logger.LogInformation(builder.ToString());
    }

    private void LogTitlesNotContainingTerm(string term, List<string> titlesThatDoNoContainTerm)
    {
        if (titlesThatDoNoContainTerm.Count == 0)
        {
            Logger.LogInformation($"All titles contained {term}");
            return;
        }

        var builder = new StringBuilder();
        builder.AppendLine($"{TitleMissingSearchTermMessage} [{term}]");
        for (var i = 0; i < titlesThatDoNoContainTerm.Count; i++)
        {
            builder.AppendLine(titlesThatDoNoContainTerm[i]);
        }

        Logger.LogError(builder.ToString());
    }
}
