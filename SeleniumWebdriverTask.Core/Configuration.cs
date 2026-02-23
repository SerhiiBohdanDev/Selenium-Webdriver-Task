// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.CoreLayer;

public class Configuration
{
    public required BrowserType BrowserType { get; init; }

    public required string ApplicationUrl { get; init; }

    public required bool Headless { get; init; }
}
