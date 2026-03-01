using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer;
using SeleniumWebdriverTask.CoreLayer.Logging;
using SeleniumWebdriverTask.CoreLayer.Utils;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.TestLayer
{
    internal abstract class TestBase
    {
        private Configuration _configuration;

        protected Logger Logger { get; private set; }

        protected DriverWrapper Driver { get; private set; }

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var config = configuration.GetSection("Configuration").Get<Configuration>();
            ArgumentNullException.ThrowIfNull(config);
            _configuration = config;
            Logger = new Logger(configuration);
        }

        [SetUp]
        public virtual void Setup()
        {
            Logger.LogInformation("Starting test");
            var browserType = _configuration.BrowserType;
            var headless = _configuration.Headless;
            var options = WebDriverOptionsFactory.CreateOptions(browserType, headless);
            AddWebDriverOptions(options);

            var driver = WebDriverFactory.CreateWebDriver(browserType, options);
            SetWebDriverSettings(driver);

            Driver = new DriverWrapper(driver, TimeSpan.FromSeconds(5));

            // Because firefox does not have argument for options.AddArgument("start-maximized"), so we maximize manually.
            Driver.Maximize(headless);
            Driver.GoToUrl(_configuration.ApplicationUrl);
        }

        [TearDown]
        public virtual void Teardown()
        {
            Logger.LogInformation("Finishing test");
            Logger.LogInformation($"Test status: {TestContext.CurrentContext.Result.Outcome.Status}");
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var screenshotLocation = ScreenshotMaker.TakeBrowserScreenshot(Driver.WebDriver);
                Logger.LogError($"Error screenshot location:\n {screenshotLocation}");
            }

            Driver.Close();
        }

        protected virtual void AddWebDriverOptions(DriverOptions options)
        {
        }

        protected virtual void SetWebDriverSettings(IWebDriver driver)
        {
        }
    }
}
