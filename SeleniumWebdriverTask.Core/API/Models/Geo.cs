// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Text.Json.Serialization;

namespace SeleniumWebdriverTask.CoreLayer.API.Models;

/// <summary>
/// Information about user's lattitude and longitude.
/// </summary>
public struct Geo
{
    /// <summary>
    /// Gets or sets lattitude.
    /// </summary>
    [JsonPropertyName("lat")]
    public string Lattitude { get; set; }

    /// <summary>
    /// Gets or sets longitude.
    /// </summary>
    [JsonPropertyName("lng")]
    public string Longitude { get; set; }
}
