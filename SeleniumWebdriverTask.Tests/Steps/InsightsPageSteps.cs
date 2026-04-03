// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using SeleniumWebdriverTask.BusinessLayer.Pages;
using SeleniumWebdriverTask.TestLayer.Contexts;
using TechTalk.SpecFlow;

namespace SeleniumWebdriverTask.TestLayer.Steps;

/// <summary>
/// Steps related to insights page.
/// </summary>
[Binding]
internal class InsightsPageSteps : BaseUiTest
{
    private readonly InsightsPage _insightsPage;
    private readonly ArticleData _articleData;

    /// <summary>
    /// Initializes a new instance of the <see cref="InsightsPageSteps"/> class.
    /// </summary>
    /// <param name="articleData">Context containing information about slide title.</param>
    public InsightsPageSteps(ArticleData articleData)
    {
        _insightsPage = new InsightsPage(Driver);
        _articleData = articleData;
    }

    /// <summary>
    /// Clicks on article link.
    /// </summary>
    /// <param name="articleName">Article name.</param>
    [When(@"I click on the button to show next slide twice and click 'More info'")]
    public void ClickNextSlideTwiceAndClickMoreInfo()
    {
        _insightsPage
                .WaitUntilPageSwitched()
                .ClickNextSlide()
                .ClickNextSlide();

        var slideTitle = FormatTitle(_insightsPage.GetActiveSlideTitle());
        _articleData.Title = slideTitle;
        _insightsPage.ClickMoreInfo();
    }

    private static string FormatTitle(string title)
    {
        // Trimming whitespace and removing non-breaking spaces.
        return title.Trim().Replace('\u00A0', ' ');
    }
}
