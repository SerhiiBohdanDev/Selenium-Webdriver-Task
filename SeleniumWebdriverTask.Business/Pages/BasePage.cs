// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.BusinessLayer.Pages;

/// <summary>
/// Base class for all pages.
/// </summary>
public abstract class BasePage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BasePage"/> class.
    /// </summary>
    /// <param name="driver">DriverWrapper instance.</param>
    protected BasePage(DriverWrapper driver)
    {
        DriverWrapper = driver;
    }

    /// <summary>
    /// Gets DriverWrapper.
    /// </summary>
    protected DriverWrapper DriverWrapper { get; private set; }
}
