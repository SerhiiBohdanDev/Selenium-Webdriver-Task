// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace SeleniumWebdriverTask.CoreLayer.WebDriver;

/// <summary>
/// A helper class to create DriverOptions.
/// </summary>
public static class WebDriverOptionsFactory
{
    /// <summary>
    /// Creates Driver options based on selected browser type and mode.
    /// </summary>
    /// <param name="browserType">The type of browser.</param>
    /// <param name="headless">Headless mode.</param>
    /// <returns>Instance of DriverOptions.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if browser type is not supported.</exception>
    public static DriverOptions CreateOptions(BrowserType browserType, bool headless)
    {
        switch (browserType)
        {
            case BrowserType.Chrome:
                {
                    var options = new ChromeOptions();
                    options.AddArgument("start-maximized");
                    if (headless)
                    {
                        options.AddArgument("--headless=new");
                        options.AddArgument("--window-size=1920,1080");
                        options.AddArgument("--disable-blink-features=AutomationControlled");
                        options.AddArgument("--disable-gpu");
                        options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                        options.AddExcludedArgument("enable-automation");
                        options.AddAdditionalOption("useAutomationExtension", false);
                        options.SetLoggingPreference(LogType.Browser, LogLevel.All);
                    }

                    return options;
                }

            case BrowserType.Edge:
                {
                    var options = new EdgeOptions();
                    options.AddArgument("start-maximized");
                    if (headless)
                    {
                        options.AddArgument("--headless=new");
                        options.AddArgument("--window-size=1920,1080");
                        options.AddArgument("--disable-blink-features=AutomationControlled");
                        options.AddArgument("--disable-gpu");
                        options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                        options.AddExcludedArgument("enable-automation");
                        options.AddAdditionalOption("useAutomationExtension", false);
                        options.SetLoggingPreference(LogType.Browser, LogLevel.All);
                    }

                    return options;
                }

            case BrowserType.Firefox:
                {
                    var options = new FirefoxOptions();
                    if (headless)
                    {
                        options.AddArgument("-headless");
                        options.AddArgument("--width=1920");
                        options.AddArgument("--height=1080");
                        options.SetPreference("dom.webdriver.enabled", false);
                        options.SetPreference("useAutomationExtension", false); // May not have effect in recent versions
                        options.SetPreference("general.useragent.override", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                        options.SetPreference("layers.acceleration.disabled", true);
                        options.LogLevel = FirefoxDriverLogLevel.Trace;
                    }

                    return options;
                }

            default:
                throw new ArgumentOutOfRangeException(nameof(browserType), browserType, null);
        }
    }

    /// <summary>
    /// Adds settings to allow downloading files.
    /// </summary>
    /// <param name="options">DriverOptions that settings will be added to.</param>
    /// <param name="downloadFolderPath">Folder in which files will be saved.</param>
    /// <returns>DriverOptions instance with new settings added.</returns>
    public static DriverOptions AddDownloadOptions(DriverOptions options, string downloadFolderPath)
    {
        if (options is ChromiumOptions chromiumOptions)
        {
            chromiumOptions.AddUserProfilePreference("download.default_directory", downloadFolderPath);
            chromiumOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromiumOptions.AddUserProfilePreference("download.directory_upgrade", true);
            chromiumOptions.AddUserProfilePreference("safebrowsing.enabled", true); // Optional, helps with some security prompts
            chromiumOptions.AddUserProfilePreference("profile.default_content_settings.popups", 0);
        }
        else if (options is FirefoxOptions firefoxOptions)
        {
            firefoxOptions.SetPreference("browser.download.dir", downloadFolderPath);
            firefoxOptions.SetPreference("browser.download.folderList", 2);
            firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf,application/zip,application/octet-stream"); // Add MIME types as needed
            firefoxOptions.SetPreference("browser.download.manager.showWhenStarting", false);
            firefoxOptions.SetPreference("pdfjs.disabled", true); // Disable built-in PDF viewer
        }

        return options;
    }
}
