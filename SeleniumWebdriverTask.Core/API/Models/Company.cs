// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Text.Json.Serialization;

namespace SeleniumWebdriverTask.CoreLayer.API.Models;

public struct Company
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("catchPhrase")]
    public string CatchPhrase { get; set; }

    [JsonPropertyName("bs")]
    public string Bs { get; set; }
}
