// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Text.RegularExpressions;
using SeleniumWebdriverTask.BusinessLayer.Pages;
using TechTalk.SpecFlow;

namespace SeleniumWebdriverTask.TestLayer.Steps;

/// <summary>
/// Steps related to insights page.
/// </summary>
[Binding]
internal class SearchPageSteps : CommonSteps
{
    private const string JobDescriptionMissingKeywordMessage = "Job description is missing the following keyword(s):";

    private readonly SearchJobsPage _searchPage;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPageSteps"/> class.
    /// </summary>
    /// <param name="articleData">Context containing information about slide title.</param>
    public SearchPageSteps()
    {
        _searchPage = new SearchJobsPage(Driver);
    }

    /// <summary>
    /// Enters search language, location, clicks remote checkbox and clicks search.
    /// </summary>
    /// <param name="languages">Language to search for.</param>
    /// <param name="location">Selected location.</param>
    [When(@"I input search '(.*)', '(.*)' and select remote option")]
    public void EnterSearchDataIntoForm(string[] languages, string location)
    {
        var keywords = string.Join(',', languages);
        Logger.LogInformation($"Starting job search, search language = {languages[0]}," +
            $" keywords to find in description [{keywords}], " +
            $"location = {location}");

        _searchPage
                .WaitUntilCountryDetected()
                .EnterLanguage(languages[0])
                .EnterLocation(location)
                .ClickRemoteCheckbox()
                .ClickSearch();
    }

    /// <summary>
    /// Checks that the last job in search results contains the language.
    /// </summary>
    /// <param name="languages">Languages array.</param>
    [Then(@"The last job in the search results should contain the searched '(.*)'")]
    public void CheckLastJobInListContainginSearchKeyword(string[] languages)
    {
        // Waiting for firefox to load the page correctly.
        Driver.WaitForCondition(() =>
        {
            return Driver.Url.Contains("search")
            && Driver.Url.Contains("country")
            && Driver.Url.Contains("vacancy_type");
        });

        Logger.LogInformation($"Current url:\n {Driver.Url}");

        var jobInformation = _searchPage.GetJobInformation();
        var isInformationContainsLanguage = false;
        for (var i = 0; i < languages.Length; i++)
        {
            var pattern = @$"\b{languages[i]}\b";
            if (Regex.IsMatch(jobInformation, pattern, RegexOptions.IgnoreCase))
            {
                isInformationContainsLanguage = true;
                break;
            }
        }

        if (!isInformationContainsLanguage)
        {
            LogJobInformationError(languages);
        }

        LogJobInformation(jobInformation);

        Assert.That(isInformationContainsLanguage, Is.True);
    }

    /// <summary>
    /// Helper method that allows using string[] for parameters in steps.
    /// </summary>
    /// <param name="languages">Languages.</param>
    /// <returns>Array of languages.</returns>
    [StepArgumentTransformation]
    public string[] TransformToArrayOfString(string languages)
    {
        return languages.Split(",");
    }

    private static void LogJobInformation(string jobInformation)
    {
        Logger.LogInformation("Job information:\n" + jobInformation);
    }

    private static void LogJobInformationError(string[] languages)
    {
        var keywords = string.Join(',', languages);
        Logger.LogError($"{JobDescriptionMissingKeywordMessage} [{keywords}]");
    }
}
