// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Extensions;

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
        else
        {
            driver.TakeScreenshot().SaveAsFile(screenshotPath);
        }

        return screenshotPath;
    }
}
