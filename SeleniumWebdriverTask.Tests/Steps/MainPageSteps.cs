// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using SeleniumWebdriverTask.BusinessLayer.Pages;
using TechTalk.SpecFlow;

namespace SeleniumWebdriverTask.TestLayer.Steps;

/// <summary>
/// Steps related to main page.
/// </summary>
[Binding]
internal class MainPageSteps : CommonSteps
{
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
    public void IHoverOverServicesLink()
    {
        _mainPage.HoverServicesLink();
    }

    /// <summary>
    /// Clicks on article link.
    /// </summary>
    /// <param name="articleName">Article name.</param>
    [When(@"I click on link to the '(.*)' article")]
    public void IClickOnLinkToTheArticle(string articleName)
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
    public void IClickOnInsightsLink()
    {
        _mainPage.ClickInsights();
    }
}
