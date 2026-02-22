using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using SeleniumWebdriverTask.TestLayer.Models;

namespace SeleniumWebdriverTask.TestLayer
{
    [TestFixtureSource(nameof(FixtureData))]
    internal abstract class TestBase
    {
        private readonly bool _headless;

        protected TestBase(FixtureModel data)
        {
            _headless = data.IsHeadless;
        }

        protected DriverWrapper Driver { get; private set; }

        [SetUp]
        public virtual void Setup()
        {
            var browserType = (BrowserType)Enum.Parse(typeof(BrowserType), Configuration.BrowserType);
            var options = WebDriverOptionsFactory.CreateOptions(browserType, _headless);
            AddWebDriverOptions(options);

            var driver = WebDriverFactory.CreateWebDriver(browserType, options);
            SetWebDriverSettings(driver);

            Driver = new DriverWrapper(driver, TimeSpan.FromSeconds(3));

            // because firefox does not have argument for options.AddArgument("start-maximized").
            Driver.Maximize();
            Driver.GoToUrl(Configuration.AppUrl);
        }

        [TearDown]
        public virtual void Teardown()
        {
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

        private static IEnumerable<FixtureModel> FixtureData()
        {
            yield return new FixtureModel(true, "Headless");
            yield return new FixtureModel(false, "Not Headless");
        }
    }
}
