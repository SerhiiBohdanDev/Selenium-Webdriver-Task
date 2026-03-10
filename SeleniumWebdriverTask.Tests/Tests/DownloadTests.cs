using OpenQA.Selenium;
using SeleniumWebdriverTask.BusinessLayer.Pages;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.TestLayer.Tests
{
    /// <summary>
    /// Class to contain tests related to downloading a file.
    /// </summary>
    public class DownloadTests : BaseUiTest
    {
        private string _downloadFolderPath;

        /// <summary>
        /// Runs before every test.
        /// </summary>
        public override void Setup()
        {
            // Create a unique download directory for each test run for best practice
            _downloadFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_downloadFolderPath);
            Logger.LogInformation($"Createing temporary directory for file: {_downloadFolderPath}");

            base.Setup();
        }

        /// <summary>
        /// Verifies that Quick Facts file can be downloaded.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>A task object that can be awaited.</returns>
        [TestCase("EPAM_ESG_Quick_Facts.pdf")]
        public async Task EPAMQuickFacts_DownloadFile_Success(string fileName)
        {
            Logger.LogInformation($"Starting download test for file '{fileName}'");
            new MainPage(Driver)
                .ClickCorporateResponsibility();

            new CorporateResponsibilityPage(Driver)
                .ScrollToDownloadLink()
                .ClickDownloadLink();

            Logger.LogInformation($"File will be saved at: {_downloadFolderPath}");
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var isFileDownloaded = await DriverWrapper.WaitForFileToFinishChangingContentAsync(_downloadFolderPath, fileName, 1, cancellationTokenSource.Token);

            Assert.That(isFileDownloaded, Is.True);
        }

        /// <summary>
        /// Runs after every test.
        /// </summary>
        public override void Teardown()
        {
            Logger.LogInformation($"Deleting directory: {_downloadFolderPath}");
            Directory.Delete(_downloadFolderPath, true);
            base.Teardown();
        }

        /// <summary>
        /// Adds options specific for downloading files.
        /// </summary>
        /// <param name="options">DriverOptions to change.</param>
        protected override void AddWebDriverOptions(DriverOptions options)
        {
            WebDriverOptionsFactory.AddDownloadOptions(options, _downloadFolderPath);
            base.AddWebDriverOptions(options);
        }

        /// <summary>
        /// Sets settings specific for downloading files.
        /// </summary>
        /// <param name="driver">IWebDriver instance.</param>
        protected override void SetWebDriverSettings(IWebDriver driver)
        {
            WebDriverFactory.SetupChromiumDriverDownloadSettings(driver, _downloadFolderPath);
            base.SetWebDriverSettings(driver);
        }
    }
}
