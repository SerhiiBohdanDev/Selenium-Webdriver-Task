// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using Newtonsoft.Json;

namespace SeleniumWebdriverTask.CoreLayer.API.Models;

/// <summary>
/// Class representing a user.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets user's id.
    /// </summary>
    [JsonRequired]
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets user's name.
    /// </summary>
    [JsonRequired]
    [JsonProperty("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets user's username.
    /// </summary>
    [JsonRequired]
    [JsonProperty("username")]
    public required string Username { get; set; }

    /// <summary>
    /// Gets or sets user's email.
    /// </summary>
    [JsonRequired]
    [JsonProperty("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets user's address.
    /// </summary>
    [JsonRequired]
    [JsonProperty("address")]
    public Address Address { get; set; }

    /// <summary>
    /// Gets or sets user's phone.
    /// </summary>
    [JsonRequired]
    [JsonProperty("phone")]
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets user's website.
    /// </summary>
    [JsonRequired]
    [JsonProperty("website")]
    public string? Website { get; set; }

    /// <summary>
    /// Gets or sets user's company.
    /// </summary>
    [JsonRequired]
    [JsonProperty("company")]
    public Company Company { get; set; }

    /// <summary>
    /// Converts struct to string.
    /// </summary>
    /// <returns>String representation of the object.</returns>
    public override string ToString() => $"User. Id: {Id}, Name: {Name}, Username: {Username}, Email: {Email}," +
        $"{Address}, Phone: {Phone}, Website: {Website}, {Company}";
}
