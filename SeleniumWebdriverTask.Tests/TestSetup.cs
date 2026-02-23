// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using Microsoft.Extensions.Configuration;
using SeleniumWebdriverTask.CoreLayer;

namespace SeleniumWebdriverTask.TestLayer;

[SetUpFixture]
internal class TestSetup
{
    public static Configuration Configuration { get; private set; }

    public static Logger Logger { get; private set; }

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var config = configuration.GetSection("Configuration").Get<Configuration>();
        ArgumentNullException.ThrowIfNull(config);
        Configuration = config;
        Logger = new Logger(configuration);
    }
}
