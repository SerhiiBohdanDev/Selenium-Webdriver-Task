// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Text.Json.Serialization;

namespace SeleniumWebdriverTask.CoreLayer.API.Models;

public struct Geo
{
    [JsonPropertyName("lat")]
    public string Lattitude { get; set; }

    [JsonPropertyName("lng")]
    public string Longitude { get; set; }
}
