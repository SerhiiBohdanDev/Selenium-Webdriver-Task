// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using Microsoft.Extensions.Configuration;
using SeleniumWebdriverTask.CoreLayer.Configurations;
using SeleniumWebdriverTask.CoreLayer.Logging;
using SeleniumWebdriverTask.CoreLayer.Utils;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using TechTalk.SpecFlow;

namespace SeleniumWebdriverTask.TestLayer.Steps;

/// <summary>
/// Steps common to all tests.
/// </summary>
[Binding]
internal abstract class CommonSteps
{
    /// <summary>
    /// Gets temporaty folder for downloaded files.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected static string DownloadFolderPath { get; private set; }

    /// <summary>
    /// Gets the configuration.
    /// </summary>
    protected static Configuration Configuration { get; private set; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    protected static Logger Logger { get; private set; }

    /// <summary>
    /// Gets the DriverWrapper.
    /// </summary>
    protected static DriverWrapper Driver { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// Runs once before any tests.
    /// </summary>
    [BeforeTestRun]
    public static void RunBeforeAnyTests()
    {
        // since this method is getting run even if BDD tests are not being run
        // we need to have a custom name for the file, otherwise it will be accessed from two places and will be locked
        Environment.SetEnvironmentVariable("LOG_NAME", "BDD");

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var configuration = config.GetSection("Configuration").Get<Configuration>();
        ArgumentNullException.ThrowIfNull(configuration);
        Configuration = configuration;
        Logger = new Logger(config);
    }

    /// <summary>
    /// Runs before every test.
    /// </summary>
    [BeforeScenario]
    public static void Setup()
    {
        Logger.LogInformation($"Starting '{TestContext.CurrentContext.Test.FullName}'");

        // have to add this for all tests because method is static
        // Create a unique download directory for each test run for best practice
        DownloadFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(DownloadFolderPath);
        Logger.LogInformation($"Createing temporary directory for file: {DownloadFolderPath}");

        var browserType = Configuration.BrowserType;
        var headless = Configuration.Headless;
        var options = WebDriverOptionsFactory.CreateOptions(browserType, headless);

        // have to add this for all tests because method is static
        WebDriverOptionsFactory.AddDownloadOptions(options, DownloadFolderPath);

        var driver = WebDriverFactory.CreateWebDriver(browserType, options);

        // have to add this for all tests because method is static
        WebDriverFactory.SetupChromiumDriverDownloadSettings(driver, DownloadFolderPath);

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
    [AfterScenario]
    public static void Teardown()
    {
        var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
        Logger.LogInformation("Finishing test");
        Logger.LogInformation($"Test status: {testStatus}");
        if (testStatus == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            var screenshotLocation = ScreenshotMaker.TakeFullPageScreenshot(Driver.WebDriver);
            Logger.LogError($"Error screenshot location:\n {screenshotLocation}");
        }

        Logger.LogInformation($"Deleting directory: {DownloadFolderPath}");
        Directory.Delete(DownloadFolderPath, true);

        Driver.Close();
    }
}
