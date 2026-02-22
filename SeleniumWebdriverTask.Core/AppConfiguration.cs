// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

namespace SeleniumWebdriverTask.CoreLayer;

public class AppConfiguration
{
    public required string BrowserType { get; set; }

    public required string ApplicationUrl { get; set; }
}
