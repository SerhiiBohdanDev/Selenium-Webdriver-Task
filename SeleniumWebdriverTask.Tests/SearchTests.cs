using System.Text;
using SeleniumWebdriverTask.BusinessLayer.Pages;
using SeleniumWebdriverTask.TestLayer.Models;

namespace SeleniumWebdriverTask.TestLayer
{
    internal class SearchTests : TestBase
    {
        private const string JobDescriptionMissingKeywordMessage = "Job description is missing the following keyword(s):";
        private const string TitleMissingSearchTermMessage = "Following titles are missing the following search term:";

        [TestCaseSource(nameof(JobsSearchData))]
        public void ValidKeyword_SearchLastJob_Success(JobSearchModel model)
        {
            new MainPage(Driver)
                .ClickJoinUs();

            var searchPage = new SearchJobsPage(Driver)
                .EnterLanguage(model.Language[0])
                .EnterLocation(model.Location)
                .ClickRemoteCheckbox()
                .ClickSearch();

            var jobInformation = searchPage.GetJobInformation();
            var isInformationContainsLanguage = false;
            for (int i = 0; i < jobInformation.Count; i++)
            {
                for (int k = 0; k < model.Language.Length; k++)
                {
                    if (ContainsText(jobInformation[i], model.Language[k]))
                    {
                        isInformationContainsLanguage = true;
                        break;
                    }
                }

                if (isInformationContainsLanguage)
                {
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

        [TestCase("BLOCKCHAIN")]
        [TestCase("Cloud")]
        [TestCase("Automation")]
        public void ValidTerm_GeneralSearchInTitle_Sucess(string term)
        {
            var mainPage = new MainPage(Driver)
                .ClickMagnifyingGlass()
                .EnterSearchTerm(term)
                .ClickFind();

            var titles = mainPage.GetSearchResultTitles();
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
                new() { Language = ["JavaScript", "JS", "Javascript"], Location = "Georgia" },
                new() { Language = ["C#", "c#"], Location = "Georgia" },
                new() { Language = ["Python", "python"], Location = "Georgia" },
                new() { Language = ["JavaScript", "JS", "Javascript"], Location = "Belgium" },
                new() { Language = ["C#", "c#"], Location = "Belgium" },
                new() { Language = ["Python", "python"], Location = "Belgium" },
                new() { Language = ["JavaScript", "JS", "Javascript"], Location = "Armenia" },
                new() { Language = ["C#", "c#"], Location = "Armenia" },
                new() { Language = ["Python", "python"], Location = "Armenia" },
            };

            foreach (var model in cases)
            {
                yield return new TestCaseData(model)
                    .SetName($"ValidKeyword_SearchLastJob_Success(\"{model.Language[0]}\", \"{model.Location}\")");
            }
        }

        private static bool ContainsText(string text, string target) => text.Contains(target, StringComparison.InvariantCulture);

        private static void LogTitlesNotContainingTerm(string term, List<string> titlesThatDoNoContainTerm)
        {
            if (titlesThatDoNoContainTerm.Count == 0)
            {
                Logger.LogInformation($"All titles contained {term}");
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine($"{TitleMissingSearchTermMessage} [{term}]");
            for (int i = 0; i < titlesThatDoNoContainTerm.Count; i++)
            {
                builder.AppendLine(titlesThatDoNoContainTerm[i]);
            }

            builder.AppendLine();
            Logger.LogError(builder.ToString());
        }

        private static void LogJobInformation(List<string> jobInformation)
        {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine($"Job information:");
            for (int i = 0; i < jobInformation.Count; i++)
            {
                builder.AppendLine(jobInformation[i]);
            }

            builder.AppendLine();
            Logger.LogInformation(builder.ToString());
        }

        private static void LogJobInformationError(string[] languages)
        {
            var builder = new StringBuilder();
            var keywords = string.Join(',', languages);
            builder.AppendLine();
            builder.AppendLine($"{JobDescriptionMissingKeywordMessage} [{keywords}]");
            builder.AppendLine();
            Logger.LogError(builder.ToString());
        }
    }
}
