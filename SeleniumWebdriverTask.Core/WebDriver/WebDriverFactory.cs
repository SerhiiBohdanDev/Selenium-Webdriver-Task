// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.DevTools.V144.Browser;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace SeleniumWebdriverTask.CoreLayer.WebDriver;

public static class WebDriverFactory
{
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
