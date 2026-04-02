// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

namespace SeleniumWebdriverTask.CoreLayer.Configurations;

/// <summary>
/// Stores endpoints used for API calls.
/// </summary>
public readonly struct ApiEndpoints
{
    /// <summary>
    /// Gets base url for API tests.
    /// </summary>
    public string BaseUrl { get; init; }

    /// <summary>
    /// Gets endpoint for users page.
    /// </summary>
    public string UsersEndpoint { get; init; }

    /// <summary>
    /// Gets endpoint for invalid page.
    /// </summary>
    public string InvalidEndpoint { get; init; }
}
