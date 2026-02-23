using OpenQA.Selenium;
using SeleniumWebdriverTask.BusinessLayer.Pages;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.TestLayer
{
    internal class DownloadTests : TestBase
    {
        private string _downloadFolderPath;

        public override void Setup()
        {
            // Create a unique download directory for each test run for best practice
            _downloadFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_downloadFolderPath);
            Logger.LogInformation($"Createing temporary directory for file: {_downloadFolderPath}");

            base.Setup();
        }

        [TestCase("EPAM_Corporate_Overview_Sept_25.pdf")]
        public async Task CorporateOverview_DownloadFile_Success(string fileName)
        {
            new MainPage(Driver)
                .ClickAbout();

            new AboutPage(Driver)
                .ScrollToAndClickDownload();

            Logger.LogInformation($"File will be saved at: {_downloadFolderPath}");
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var isFileDownloaded = await DriverWrapper.WaitForFileToFinishChangingContentAsync(_downloadFolderPath, fileName, 1, cancellationTokenSource.Token);

            Assert.That(isFileDownloaded, Is.True);
        }

        [TestCase("EPAM_ESG_Quick_Facts.pdf")]
        public async Task EPAMQuickFacts_DownloadFile_Success(string fileName)
        {
            new MainPage(Driver)
                .ClickCorporateResponsibility();

            new CorporateResponsibilityPage(Driver)
                .ScrollToAndClickDownload();

            Logger.LogInformation($"File will be saved at: {_downloadFolderPath}");
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            bool isFileDownloaded = await DriverWrapper.WaitForFileToFinishChangingContentAsync(_downloadFolderPath, fileName, 1, cancellationTokenSource.Token);

            Assert.That(isFileDownloaded, Is.True);
        }

        public override void Teardown()
        {
            Logger.LogInformation($"Deleting directory: {_downloadFolderPath}");
            Directory.Delete(_downloadFolderPath, true);
            base.Teardown();
        }

        protected override void AddWebDriverOptions(DriverOptions options)
        {
            WebDriverOptionsFactory.AddDownloadOptions(options, _downloadFolderPath);
            base.AddWebDriverOptions(options);
        }

        protected override void SetWebDriverSettings(IWebDriver driver)
        {
            WebDriverFactory.SetupChromiumDriverDownloadSettings(driver, _downloadFolderPath);
            base.SetWebDriverSettings(driver);
        }
    }
}
