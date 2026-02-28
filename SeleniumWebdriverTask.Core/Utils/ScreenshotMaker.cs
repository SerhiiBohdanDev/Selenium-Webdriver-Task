// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using OpenQA.Selenium;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Firefox;

namespace SeleniumWebdriverTask.CoreLayer.Utils;

public static class ScreenshotMaker
{
    private static string NewScreenshotName
    {
        get { return "_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-fff") + ".png"; }
    }

    public static string TakeBrowserScreenshot(IWebDriver driver)
    {
        var screenshotPath = Path.Combine(Environment.CurrentDirectory, $"Display_{NewScreenshotName}");
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
