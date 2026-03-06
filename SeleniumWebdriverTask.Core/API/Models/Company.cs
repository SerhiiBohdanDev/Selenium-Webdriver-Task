// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Text.Json.Serialization;

namespace SeleniumWebdriverTask.CoreLayer.API.Models;

/// <summary>
/// Information about user's company.
/// </summary>
public struct Company
{
    /// <summary>
    /// Gets or sets company name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets company catch phrase.
    /// </summary>
    [JsonPropertyName("catchPhrase")]
    public string CatchPhrase { get; set; }

    /// <summary>
    /// Gets or sets company bs.
    /// </summary>
    [JsonPropertyName("bs")]
    public string Bs { get; set; }
}
