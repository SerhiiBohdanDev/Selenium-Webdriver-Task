// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.DevTools.V144.Browser;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace SeleniumWebdriverTask.CoreLayer.WebDriver;

/// <summary>
/// A factory class to create IWebDriver.
/// </summary>
public static class WebDriverFactory
{
    /// <summary>
    /// Creates IWebDriver instance.
    /// </summary>
    /// <param name="browserType">Selected browser type.</param>
    /// <param name="options">Driver options.</param>
    /// <returns>Instance of IWebDriver.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if browser type is not supported.</exception>
    public static IWebDriver CreateWebDriver(BrowserType browserType, DriverOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        return browserType switch
        {
            BrowserType.Chrome => new ChromeDriver((ChromeOptions)options),
            BrowserType.Edge => new EdgeDriver((EdgeOptions)options),
            BrowserType.Firefox => new FirefoxDriver((FirefoxOptions)options),
            _ => throw new ArgumentOutOfRangeException(nameof(browserType), browserType, null),
        };
    }

    /// <summary>
    /// Sets additional settings for Chromium-based drivers to allow downloading.
    /// </summary>
    /// <param name="driver">Instanec of IWebDriver.</param>
    /// <param name="downloadFolderPath">Path to a folder where file will be saved.</param>
    public static void SetupChromiumDriverDownloadSettings(IWebDriver driver, string downloadFolderPath)
    {
        if (driver is IDevTools devTools)
        {
            devTools
            .GetDevToolsSession()
            .SendCommand(new SetDownloadBehaviorCommandSettings
            {
                Behavior = "allow",
                DownloadPath = downloadFolderPath,
            })
            .GetAwaiter()
            .GetResult();
        }
    }
}
