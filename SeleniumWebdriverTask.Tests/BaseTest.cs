using System.Threading;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWebdriverTask.CoreLayer;
using SeleniumWebdriverTask.CoreLayer.Logging;
using SeleniumWebdriverTask.CoreLayer.Utils;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.TestLayer
{
    /// <summary>
    /// A base class for classes containing tests.
    /// </summary>
    internal abstract class BaseTest
    {
        private Configuration _configuration;

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected Logger Logger { get; private set; }

        /// <summary>
        /// Gets the DriverWrapper.
        /// </summary>
        protected DriverWrapper Driver { get; private set; }

        /// <summary>
        /// Runs once before any tests.
        /// </summary>
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var config = configuration.GetSection("Configuration").Get<Configuration>();
            ArgumentNullException.ThrowIfNull(config);
            _configuration = config;
            Logger = new Logger(configuration);
        }

        /// <summary>
        /// Runs before every test.
        /// </summary>
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

            Console.WriteLine("_configuration.ExplicitWaitSeconds = " + _configuration.ExplicitWait);
            Driver = new DriverWrapper(driver, TimeSpan.FromSeconds(_configuration.ExplicitWait));

            // Because firefox does not have argument for options.AddArgument("start-maximized"), so we maximize manually.
            Driver.Maximize(headless);
            Driver.GoToUrl(_configuration.ApplicationUrl);
        }

        /// <summary>
        /// Runs after every test.
        /// </summary>
        [TearDown]
        public virtual void Teardown()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            Logger.LogInformation("Finishing test");
            Logger.LogInformation($"Test status: {testStatus}");
            if (testStatus == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var screenshotLocation = ScreenshotMaker.TakeFullPageScreenshot(Driver.WebDriver);
                Logger.LogError($"Error screenshot location:\n {screenshotLocation}");
            }

            Driver.Close();
        }

        /// <summary>
        /// Virutal method to allow adding options to the driver.
        /// </summary>
        /// <param name="options">Options to be added.</param>
        protected virtual void AddWebDriverOptions(DriverOptions options)
        {
        }

        /// <summary>
        /// Virtual method to allow changing settings of the driver.
        /// </summary>
        /// <param name="driver">Instance of the IWebDriver.</param>
        protected virtual void SetWebDriverSettings(IWebDriver driver)
        {
        }
    }
}
