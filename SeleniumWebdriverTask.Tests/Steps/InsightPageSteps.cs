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
internal class InsightPageSteps : BaseUiTest
{
    private readonly InsightBasePage _insightPage;
    private readonly ArticleData _articleData;

    /// <summary>
    /// Initializes a new instance of the <see cref="InsightPageSteps"/> class.
    /// </summary>
    /// <param name="articleData">Context containing information about slide title.</param>
    public InsightPageSteps(ArticleData articleData)
    {
        _insightPage = new InsightBasePage(Driver);
        _articleData = articleData;
    }

    /// <summary>
    /// Clicks on article link.
    /// </summary>
    /// <param name="articleName">Article name.</param>
    [Then(@"Browser navigates to the slide article page")]
    public void BrowserNavigatesToSlideArticlePage()
    {
        var pageTitle = FormatTitle(_insightPage.GetTitle());
        LogComparedTitles(_articleData.Title, pageTitle);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(pageTitle, Is.Not.Empty);
            Assert.That(_articleData.Title, Is.Not.Empty);
            Assert.That(pageTitle, Does.Contain(_articleData.Title));
        }
    }

    private static string FormatTitle(string title)
    {
        // Trimming whitespace and removing non-breaking spaces.
        return title.Trim().Replace('\u00A0', ' ');
    }

    private void LogComparedTitles(string slideTitle, string pageTitle)
    {
        if (pageTitle.Contains(slideTitle))
        {
            Logger.LogInformation($"Titles match \n" +
                $"Slide title = '{slideTitle}'\n" +
                $"Page title = '{pageTitle}'");
        }
        else
        {
            Logger.LogError($"Titles DO NOT match \n" +
                $"Slide title = '{slideTitle}'\n" +
                $"Page title = '{pageTitle}'");
        }
    }
}
