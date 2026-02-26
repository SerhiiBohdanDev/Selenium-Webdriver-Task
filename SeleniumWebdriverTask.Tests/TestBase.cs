using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.Logging;
using SeleniumWebdriverTask.CoreLayer.Utils;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.TestLayer
{
    internal abstract class TestBase
    {
        protected static Logger Logger => TestSetup.Logger;

        protected DriverWrapper Driver { get; private set; }

        [SetUp]
        public virtual void Setup()
        {
            Logger.LogInformation("Starting test");
            var browserType = TestSetup.Configuration.BrowserType;
            var headless = TestSetup.Configuration.Headless;
            var options = WebDriverOptionsFactory.CreateOptions(browserType, headless);
            AddWebDriverOptions(options);

            var driver = WebDriverFactory.CreateWebDriver(browserType, options);
            SetWebDriverSettings(driver);

            Driver = new DriverWrapper(driver, TimeSpan.FromSeconds(5));

            // because firefox does not have argument for options.AddArgument("start-maximized").
            Driver.Maximize(headless);
            Driver.GoToUrl(TestSetup.Configuration.ApplicationUrl);
        }

        [TearDown]
        public virtual void Teardown()
        {
            Logger.LogInformation("Finishing test");
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var screenshotLocation = ScreenshotMaker.TakeBrowserScreenshot(Driver.WebDriver);
                Logger.LogError($"Error screenshot:\n {screenshotLocation}");
            }

            Driver.Close();
        }

        protected static void LogInformation(string text)
        {
            Console.WriteLine(text);
        }

        protected virtual void AddWebDriverOptions(DriverOptions options)
        {
        }

        protected virtual void SetWebDriverSettings(IWebDriver driver)
        {
        }
    }
}
