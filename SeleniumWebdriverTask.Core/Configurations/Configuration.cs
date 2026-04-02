// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.CoreLayer.Configurations;

/// <summary>
/// A model class to use with json configuration.
/// </summary>
public class Configuration
{
    /// <summary>
    /// Gets or sets type of browser.
    /// </summary>
    public required BrowserType BrowserType { get; set; }

    /// <summary>
    /// Gets application main url.
    /// </summary>
    public required string ApplicationUrl { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether browser is in headless mode.
    /// </summary>
    public required bool Headless { get; set; }

    /// <summary>
    /// Gets explicit wait duration in seconds.
    /// </summary>
    public required double ExplicitWait { get; init; }

    /// <summary>
    /// Gets base url for API tests.
    /// </summary>
    public required string ApiTestsBaseUrl { get; init; }

    /// <summary>
    /// Gets endpoint for users page.
    /// </summary>
    public required string UsersEndpoint { get; init; }

    /// <summary>
    /// Gets endpoint for invalid page.
    /// </summary>
    public required string InvalidEndpoint { get; init; }

    /// <summary>
    /// Gets endpoints for API calls.
    /// </summary>
    public required ApiEndpoints ApiEndpoints { get; init; }
}
