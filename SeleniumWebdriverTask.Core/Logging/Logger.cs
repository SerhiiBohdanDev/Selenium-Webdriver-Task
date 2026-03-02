// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using Microsoft.Extensions.Configuration;
using Serilog;

namespace SeleniumWebdriverTask.CoreLayer.Logging;

/// <summary>
/// A class used to wrap a logger instance.
/// </summary>
public class Logger
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Logger"/> class.
    /// </summary>
    /// <param name="configuration">Configuration to control logger.</param>
    public Logger(IConfiguration configuration)
    {
        _logger = new LoggerConfiguration()
                .ReadFrom
                .Configuration(configuration)
                .CreateLogger();
    }

    /// <summary>
    /// Writes 'Information' level event in log.
    /// </summary>
    /// <param name="message">Message to log.</param>
    public void LogInformation(string message) => _logger.Information(message);

    /// <summary>
    /// Writes 'Error' level event in log.
    /// </summary>
    /// <param name="message">Message to log.</param>
    public void LogError(string message) => _logger.Error(message);
}
