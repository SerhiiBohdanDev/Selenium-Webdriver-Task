using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.TestLayer.BaseTests
{
    /// <summary>
    /// Base class to setup downloading files.
    /// </summary>
    public abstract class BaseDownloadTest : BaseUiTest
    {
        /// <summary>
        /// Gets the path to folder where files will be downloaded.
        /// </summary>
        protected string DownloadFolderPath { get; private set; }

        /// <summary>
        /// Runs before every test.
        /// </summary>
        public override void Setup()
        {
            // Create a unique download directory for each test run for best practice
            DownloadFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(DownloadFolderPath);
            Logger.LogInformation($"Createing temporary directory for file: {DownloadFolderPath}");

            base.Setup();
        }

        /// <summary>
        /// Runs after every test.
        /// </summary>
        public override void Teardown()
        {
            Logger.LogInformation($"Deleting directory: {DownloadFolderPath}");
            Directory.Delete(DownloadFolderPath, true);
            base.Teardown();
        }

        /// <summary>
        /// Adds options specific for downloading files.
        /// </summary>
        /// <param name="options">DriverOptions to change.</param>
        protected override void AddWebDriverOptions(DriverOptions options)
        {
            WebDriverOptionsFactory.AddDownloadOptions(options, DownloadFolderPath);
            base.AddWebDriverOptions(options);
        }

        /// <summary>
        /// Sets settings specific for downloading files.
        /// </summary>
        /// <param name="driver">IWebDriver instance.</param>
        protected override void SetWebDriverSettings(IWebDriver driver)
        {
            WebDriverFactory.SetupChromiumDriverDownloadSettings(driver, DownloadFolderPath);
            base.SetWebDriverSettings(driver);
        }
    }
}
