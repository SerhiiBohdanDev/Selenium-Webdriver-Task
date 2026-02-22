// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using Microsoft.Extensions.Configuration;

namespace SeleniumWebdriverTask.CoreLayer;

public static class Configuration
{
    static Configuration()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        BrowserType = configuration["BrowserType"] ?? "Chrome";
        AppUrl = configuration["ApplicationUrl"] ?? string.Empty;
    }

    public static string BrowserType { get; private set; }

    public static string AppUrl { get; private set; }
}
