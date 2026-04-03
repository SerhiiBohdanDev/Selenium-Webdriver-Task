using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.Logging;
using SeleniumWebdriverTask.CoreLayer.Utils;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.TestLayer.BaseTests
{
    /// <summary>
    /// A base class for tests using WebDriver.
    /// </summary>
    [Category("BddUI")]
    public abstract class BaseBddUiTest : BaseTest
    {
        /// <summary>
        /// Gets the DriverWrapper.
        /// </summary>
        protected DriverWrapper Driver { get; private set; }

        /// <summary>
        /// Runs before every test.
        /// </summary>
        public override void Setup()
        {
            base.Setup();
            var browserType = Configuration.BrowserType;
            var headless = Configuration.Headless;
            var options = WebDriverOptionsFactory.CreateOptions(browserType, headless);
            AddWebDriverOptions(options);

            var driver = WebDriverFactory.CreateWebDriver(browserType, options);
            SetWebDriverSettings(driver);

            Driver = new DriverWrapper(driver, TimeSpan.FromSeconds(Configuration.ExplicitWait));

            // maximization in headless mode resets screen size to default 800x600
            if (!headless)
            {
                Driver.Maximize();
            }

            Driver.GoToUrl(Configuration.ApplicationUrl);
        }

        /// <summary>
        /// Runs after every test.
        /// </summary>
        public override void Teardown()
        {
            base.Teardown();
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
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
