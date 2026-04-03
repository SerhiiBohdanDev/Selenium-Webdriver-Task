// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using SeleniumWebdriverTask.BusinessLayer.Pages;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.TestLayer.BaseTests;
using TechTalk.SpecFlow;

namespace SeleniumWebdriverTask.TestLayer.Steps;

/// <summary>
/// Steps related to insights page.
/// </summary>
[Binding]
[Category("BddUI")]
internal class CorporateResponsibilityPageSteps : BaseDownloadTest
{
    private readonly CorporateResponsibilityPage _corporateResponsibilityPage;

    /// <summary>
    /// Initializes a new instance of the <see cref="CorporateResponsibilityPageSteps"/> class.
    /// </summary>
    public CorporateResponsibilityPageSteps()
    {
        _corporateResponsibilityPage = new CorporateResponsibilityPage(Driver);
    }

    /// <summary>
    /// Scroll down and click download link.
    /// </summary>
    [When(@"I scroll down and click the download link on 'Corporate responsibility' page")]
    public void BrowserNavigatesToSlideArticlePage()
    {
        _corporateResponsibilityPage
            .ScrollToDownloadLink()
            .ClickDownloadLink();
    }

    /// <summary>
    /// Verifies that file can be downloaded.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>A task object that can be awaited.</returns>
    [Then(@"Browser successfully downloads the file '(.*)'")]
    public async Task BrowserDownloadsFile(string fileName)
    {
        Logger.LogInformation($"File will be saved at: {DownloadFolderPath}");
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        var isFileDownloaded = await DriverWrapper.WaitForFileToFinishChangingContentAsync(DownloadFolderPath, fileName, 1, cancellationTokenSource.Token);
        Assert.That(isFileDownloaded, Is.True);
    }
}
