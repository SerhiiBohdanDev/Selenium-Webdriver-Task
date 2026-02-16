using System.Text;
using LocatorsForWebElements.BusinessLayer.Pages;
using LocatorsForWebElements.CoreLayer;
using LocatorsForWebElements.TestLayer.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.DevTools.V144.Page;

namespace LocatorsForWebElements.TestLayer
{
    [TestFixtureSource(nameof(FixtureData))]
    internal class Tests
    {
        private const string JobDescriptionMissingKeywordMessage = "Job description is missing the following keyword(s):";
        private const string TitleMissingSearchTermMessage = "Following titles are missing the following search term:";

        private readonly bool _headless;
        private DriverWrapper _driver;
        private string _downloadFolderPath;

        public Tests(FixtureModel data)
        {
            _headless = data.IsHeadless;
        }

        [SetUp]
        public void Setup()
        {
            // Create a unique download directory for each test run for best practice
            _downloadFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_downloadFolderPath);
            LogInformation(_downloadFolderPath);

            var options = new ChromeOptions();
            options.AddArgument("start-maximized");
            if (_headless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--window-size=1920,1080");
                options.AddArgument("--disable-blink-features=AutomationControlled");
                options.AddExcludedArgument("enable-automation");
                options.AddAdditionalOption("useAutomationExtension", false);
                options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                options.SetLoggingPreference(LogType.Browser, LogLevel.All);
            }

            options.AddUserProfilePreference("download.default_directory", _downloadFolderPath);
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.directory_upgrade", true);
            options.AddUserProfilePreference("safebrowsing.enabled", true); // Optional, helps with some security prompts
            options.AddUserProfilePreference("profile.default_content_settings.popups", 0);

            var driver = new ChromeDriver(options);
            var devTools = driver as IDevTools;
            var session = devTools.GetDevToolsSession();
            session.SendCommand(new SetDownloadBehaviorCommandSettings
            {
                Behavior = "allow",
                DownloadPath = _downloadFolderPath,
            }).GetAwaiter().GetResult();
            _driver = new DriverWrapper(driver, TimeSpan.FromSeconds(3));
        }

        [TestCaseSource(nameof(JobsSearchData))]
        public void SearchJobsTest(JobSearchModel model)
        {
            new MainPage(_driver)
                .Open()
                .ClickJoinUs();

            var searchPage = new SearchJobsPage(_driver)
                .EnterLanguage(model.Language)
                .EnterLocation(model.Location)
                .ClickRemoteCheckbox()
                .ClickSearch();

            List<string> jobInformation = searchPage.GetJobInformation();
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
                LogJobNotContainingLanguage(model.Language, jobInformation);
            }

            Assert.That(isInformationContainsLanguage, Is.True);
        }

        [TestCase("BLOCKCHAIN")]
        [TestCase("Cloud")]
        [TestCase("Automation")]
        public void GeneralSearchTest(string term)
        {
            var mainPage = new MainPage(_driver)
                .Open()
                .ClickMagnifyingGlass()
                .EnterSearchTerm(term)
                .ClickFind();

            List<string> titles = mainPage.GetSearchResultTitles();
            var allTitlesContainTerm = true;
            List<string> titlesNotContainingTerm = [];
            foreach (var title in titles.Where(title => !title.Contains(term, StringComparison.InvariantCultureIgnoreCase)))
            {
                allTitlesContainTerm = false;
                titlesNotContainingTerm.Add(title);
            }

            if (!allTitlesContainTerm)
            {
                LogTitlesNotContainingTerm(term, titlesNotContainingTerm);
            }

            Assert.That(allTitlesContainTerm, Is.True);
        }

        [TestCase("EPAM_Corporate_Overview_Sept_25.pdf")]
        public async Task DownloadFileTest(string fileName)
        {
            new MainPage(_driver)
                .Open()
                .ClickAbout();

            Console.WriteLine($"after clicking about I am on {_driver.Url}");
            new AboutPage(_driver)
                .ScrollToAndClickDownload();

            Console.WriteLine($"currently on {_driver.Url}");

            string filePath = Path.Combine(_downloadFolderPath, fileName);
            LogInformation(filePath);
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            bool isFileDownloaded = await DriverWrapper.WaitForFileToFinishChangingContentAsync(filePath, 1, cancellationTokenSource.Token);

            Assert.That(isFileDownloaded, Is.True);
        }

        [Test]
        public void SlideInformationTest()
        {
            new MainPage(_driver)
                .Open()
                .ClickInsights();

            var insightsPage = new InsightsPage(_driver);
            insightsPage
                .ClickNextSlide()
                .ClickNextSlide();

            string slideTitle = insightsPage.GetActiveSlideTitle();
            insightsPage.ClickMoreInfo();

            var insightPage = new InsightBasePage(_driver);
            string pageTitle = insightPage.GetTitle();

            LogComparedTitles(slideTitle, pageTitle);

            Assert.That(pageTitle, Does.Contain(slideTitle));
        }

        [TearDown]
        public void Teardown()
        {
            Directory.Delete(_downloadFolderPath, true);
            _driver.Close();
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
                    .SetName($"SearchJobsTest(\"{model.Language[0]}\", \"{model.Location}\")");
            }
        }

        private static IEnumerable<FixtureModel> FixtureData()
        {
            yield return new FixtureModel(true, "Headless");
            yield return new FixtureModel(false, "Not Headless");
        }

        private static bool ContainsText(string text, string target) => text.Contains(target, StringComparison.InvariantCulture);

        private static void LogInformation(string text)
        {
            Console.WriteLine(text);
        }

        private static void LogTitlesNotContainingTerm(string term, List<string> titlesThatDoNoContainTerm)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"{TitleMissingSearchTermMessage} [{term}]");
            builder.AppendLine();
            for (int i = 0; i < titlesThatDoNoContainTerm.Count; i++)
            {
                builder.AppendLine(titlesThatDoNoContainTerm[i]);
            }

            LogInformation(builder.ToString());
        }

        private static void LogJobNotContainingLanguage(string[] languages, List<string> jobInformation)
        {
            var builder = new StringBuilder();
            string keywords = string.Join(',', languages);

            builder.AppendLine($"{JobDescriptionMissingKeywordMessage} [{keywords}]");
            builder.AppendLine();
            for (int i = 0; i < jobInformation.Count; i++)
            {
                builder.AppendLine(jobInformation[i]);
            }

            LogInformation(builder.ToString());
        }

        private static void LogComparedTitles(string slideTitle, string pageTitle)
        {
            LogInformation($"Slide title = {slideTitle}\n" +
                $"Page title = {pageTitle}");
        }
    }
}
