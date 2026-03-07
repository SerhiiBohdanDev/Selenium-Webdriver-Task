// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Text.Json.Serialization;

namespace SeleniumWebdriverTask.CoreLayer.API.Models;

/// <summary>
/// Information about user address.
/// </summary>
public struct Address
{
    /// <summary>
    /// Gets or sets street.
    /// </summary>
    [JsonPropertyName("street")]
    public string Street { get; set; }

    /// <summary>
    /// Gets or sets suite.
    /// </summary>
    [JsonPropertyName("suite")]
    public string Suite { get; set; }

    /// <summary>
    /// Gets or sets city.
    /// </summary>
    [JsonPropertyName("city")]
    public string City { get; set; }

    /// <summary>
    /// Gets or sets zipcode.
    /// </summary>
    [JsonPropertyName("zipcode")]
    public string Zipcode { get; set; }

    /// <summary>
    /// Gets or sets lattitude and longitude.
    /// </summary>
    [JsonPropertyName("geo")]
    public Geo Geo { get; set; }

    /// <summary>
    /// Converts struct to string.
    /// </summary>
    /// <returns>String representation of the object.</returns>
    public override readonly string ToString() => $"Address. " +
        $"Street: {Street}, Suite: {Suite}, City: {City}, Zipcode: {Zipcode}, {Geo}";
}
