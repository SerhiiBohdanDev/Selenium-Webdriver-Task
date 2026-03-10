// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using OpenQA.Selenium;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Firefox;

namespace SeleniumWebdriverTask.CoreLayer.Utils;

/// <summary>
/// A helper class to make screenshots.
/// </summary>
public static class ScreenshotMaker
{
    private const string ScreenshotsFolder = "Screenshots";

    private static string NewScreenshotName
    {
        get { return "Screenshot_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-fff") + ".png"; }
    }

    /// <summary>
    /// Takes full page screenshot of a web page.
    /// </summary>
    /// <param name="driver">Instance of IWebDriver.</param>
    /// <returns>Path where screenshot is saved.</returns>
    /// <exception cref="NotSupportedException">Thrown if type of IWebDiver is not supported.</exception>
    public static string TakeFullPageScreenshot(IWebDriver driver)
    {
        var screenshotsFolder = Path.Combine(Environment.CurrentDirectory, ScreenshotsFolder);
        var screenshotPath = Path.Combine(screenshotsFolder, NewScreenshotName);
        Directory.CreateDirectory(screenshotsFolder);
        if (driver is FirefoxDriver firefoxDriver)
        {
            firefoxDriver.GetFullPageScreenshot().SaveAsFile(screenshotPath);
        }
        else if (driver is ChromiumDriver chromiumDriver)
        {
            var metrics = (Dictionary<string, object>?)chromiumDriver.ExecuteCdpCommand("Page.getLayoutMetrics", []);
            if (metrics == null)
            {
                return "Executing cdp command 'Page.getLayoutMetrics' returned null";
            }

            var contentSize = (Dictionary<string, object>?)metrics["contentSize"];
            if (contentSize == null)
            {
                return "Accessing 'contentSize' of Page.getLayoutMetrics returned null";
            }

            var width = contentSize["width"];
            var height = contentSize["height"];

            chromiumDriver.ExecuteCdpCommand(
                "Emulation.setDeviceMetricsOverride",
                new Dictionary<string, object>()
                {
                    { "mobile", false },
                    { "width", width },
                    { "height", height },
                    { "deviceScaleFactor", 1 },
                });

            var screenshot = (Dictionary<string, object>?)chromiumDriver.ExecuteCdpCommand(
                "Page.captureScreenshot",
                new Dictionary<string, object>() { { "fromSurface", true } });
            chromiumDriver.ExecuteCdpCommand("Emulation.clearDeviceMetricsOverride", []);

            if (screenshot == null)
            {
                return "Executing cdp command 'Page.captureScreenshot' returned null";
            }

            var imageBytes = Convert.FromBase64String((string)screenshot["data"]);
            File.WriteAllBytes(screenshotPath, imageBytes);
        }
        else
        {
            throw new NotSupportedException("The type of driver is not supported.");
        }

        return screenshotPath;
    }
}
