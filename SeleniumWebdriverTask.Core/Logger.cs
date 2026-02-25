// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using Microsoft.Extensions.Configuration;
using Serilog;

namespace SeleniumWebdriverTask.CoreLayer;

public class Logger
{
    private readonly ILogger _logger;

    public Logger(IConfiguration configuration)
    {
        _logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
    }

    public void LogInformation(string message) => _logger.Information(message);

    public void LogError(string message) => _logger.Error(message);
}
