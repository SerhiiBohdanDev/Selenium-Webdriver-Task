// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using Microsoft.Extensions.Configuration;
using SeleniumWebdriverTask.CoreLayer;
using SeleniumWebdriverTask.CoreLayer.Logging;
using SeleniumWebdriverTask.CoreLayer.Utils;
using SeleniumWebdriverTask.CoreLayer.WebDriver;
using TechTalk.SpecFlow;

namespace SeleniumWebdriverTask.TestLayer.Steps;

/// <summary>
/// Steps common to all tests.
/// </summary>
[Binding]
internal class CommonSteps
{
    private static string s_downloadFolderPath;

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

    /// <summary>
    /// Runs once before any tests.
    /// </summary>
    [BeforeTestRun]
    public static void RunBeforeAnyTests()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
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
        s_downloadFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(s_downloadFolderPath);
        Logger.LogInformation($"Createing temporary directory for file: {s_downloadFolderPath}");

        var browserType = Configuration.BrowserType;
        var headless = Configuration.Headless;
        var options = WebDriverOptionsFactory.CreateOptions(browserType, headless);

        // have to add this for all tests because method is static
        WebDriverOptionsFactory.AddDownloadOptions(options, s_downloadFolderPath);

        var driver = WebDriverFactory.CreateWebDriver(browserType, options);

        // have to add this for all tests because method is static
        WebDriverFactory.SetupChromiumDriverDownloadSettings(driver, s_downloadFolderPath);

        Driver = new DriverWrapper(driver, TimeSpan.FromSeconds(Configuration.ExplicitWait));

        // Because firefox does not have argument for options.AddArgument("start-maximized"), so we maximize manually.
        Driver.Maximize(headless);
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

        Logger.LogInformation($"Deleting directory: {s_downloadFolderPath}");
        Directory.Delete(s_downloadFolderPath, true);

        Driver.Close();
    }
}
