using System.Text;
using System.Text.RegularExpressions;
using SeleniumWebdriverTask.BusinessLayer.Pages;

namespace SeleniumWebdriverTask.TestLayer.Tests
{
    /// <summary>
    /// Test related to searching functionality.
    /// </summary>
    [Category("NUnitUiTests")]
    public class SearchTests : BaseUiTest
    {
        private const string JobDescriptionMissingKeywordMessage = "Job description is missing the following keyword(s):";
        private const string TitleMissingSearchTermMessage = "Following titles are missing the following search term:";

        /// <summary>
        /// Validates that last job in the search results contains the searched term.
        /// </summary>
        /// <param name="model">Model class containing information about search.</param>
        [TestCaseSource(nameof(JobsSearchData))]
        public void ValidKeyword_SearchLastJob_LastJobInSearchResultsContainsKeyword(JobSearchModel model)
        {
            var keywords = string.Join(',', model.Language);
            Logger.LogInformation($"Starting job search, search language = {model.Language[0]}," +
                $" keywords to find in description [{keywords}], " +
                $"location = {model.Location}");

            new MainPage(Driver)
                .ClickJoinUs();

            var searchPage = new SearchJobsPage(Driver);
            searchPage
                .WaitUntilCountryDetected()
                .EnterLanguage(model.Language[0])
                .EnterLocation(model.Location)
                .ClickRemoteCheckbox()
                .ClickSearch();

            // Waiting for firefox to load the page correctly.
            Driver.WaitForCondition(() =>
            {
                return Driver.Url.Contains("search")
                && Driver.Url.Contains("country")
                && Driver.Url.Contains("vacancy_type");
            });

            Logger.LogInformation($"Current url:\n {Driver.Url}");

            var jobInformation = searchPage.GetJobInformation();
            var isInformationContainsLanguage = false;
            for (var i = 0; i < model.Language.Length; i++)
            {
                var pattern = @$"\b{model.Language[i]}\b";
                if (Regex.IsMatch(jobInformation, pattern, RegexOptions.IgnoreCase))
                {
                    isInformationContainsLanguage = true;
                    break;
                }
            }

            if (!isInformationContainsLanguage)
            {
                LogJobInformationError(model.Language);
            }

            LogJobInformation(jobInformation);
            Assert.That(isInformationContainsLanguage, Is.True);
        }

        /// <summary>
        /// Verifies that search result titles contain the search term.
        /// </summary>
        /// <param name="term">Term to seach for.</param>
        [TestCase("BLOCKCHAIN")]
        [TestCase("Cloud")]
        [TestCase("Automation")]
        public void ValidTerm_GeneralSearchInTitle_AllTitlesContainSearchTerm(string term)
        {
            Logger.LogInformation($"Starting main page search, searching for '{term}'");
            var mainPage = new MainPage(Driver)
                .ClickMagnifyingGlass()
                .EnterSearchTerm(term)
                .ClickFind();

            var titles = mainPage.GetSearchResultTitles();
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

        private static IEnumerable<TestCaseData> JobsSearchData()
        {
            var cases = new JobSearchModel[]
            {
                new(["JavaScript", "js"], "Georgia"),
                new(["JavaScript", "js"], "Armenia"),
                new(["NET"], "Georgia"),
                new(["NET"], "Armenia"),
                new(["Python"], "Georgia"),
                new(["Python"], "Armenia"),
            };

            foreach (var model in cases)
            {
                yield return new TestCaseData(model)
                    .SetName($"ValidKeyword_SearchLastJob_Success(\"{model.Language[0]}\", \"{model.Location}\")");
            }
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

        private void LogJobInformation(string jobInformation)
        {
            Logger.LogInformation("Job information:\n" + jobInformation);
        }

        private void LogJobInformationError(string[] languages)
        {
            var keywords = string.Join(',', languages);
            Logger.LogError($"{JobDescriptionMissingKeywordMessage} [{keywords}]");
        }

        /// <summary>
        /// A model class to be used for TestCaseSource in job search tests.
        /// </summary>
        public record JobSearchModel(string[] Language, string Location);
    }
}
