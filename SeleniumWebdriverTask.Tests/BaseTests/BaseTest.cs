using Microsoft.Extensions.Configuration;
using SeleniumWebdriverTask.CoreLayer.Configurations;
using SeleniumWebdriverTask.CoreLayer.Logging;

namespace SeleniumWebdriverTask.TestLayer.BaseTests
{
    /// <summary>
    /// A base class for classes containing tests.
    /// </summary>
    public abstract class BaseTest
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        protected Configuration Configuration { get; private set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected Logger Logger { get; private set; }

        /// <summary>
        /// Runs once before any tests.
        /// </summary>
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var configuration = config.GetSection("Configuration").Get<Configuration>();
            ArgumentNullException.ThrowIfNull(configuration);

            Configuration = configuration;
            Logger = new Logger(config);
            Logger.LogInformation($"Setup browser: {Configuration.BrowserType}, Headless: {Configuration.Headless}");
        }

        /// <summary>
        /// Runs before every test.
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            Logger.LogInformation($"Starting '{TestContext.CurrentContext.Test.FullName}'");
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
        }
    }
}
