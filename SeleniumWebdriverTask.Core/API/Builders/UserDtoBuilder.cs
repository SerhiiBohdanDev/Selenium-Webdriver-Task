// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using SeleniumWebdriverTask.CoreLayer.API.Models;

namespace SeleniumWebdriverTask.CoreLayer.API.Builders;

/// <summary>
/// Class allowing fluent building of a user object.
/// </summary>
public class UserDtoBuilder
{
    private string _name = "Default name";
    private string _username = "Default username";

    /// <summary>
    /// Sets user's name.
    /// </summary>
    /// <param name="name">New name.</param>
    /// <returns>UserDtoBuilder instence.</returns>
    public UserDtoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    /// <summary>
    /// Sets user's username.
    /// </summary>
    /// <param name="username">New username.</param>
    /// <returns>UserDtoBuilder instence.</returns>
    public UserDtoBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    /// <summary>
    /// Creates user object.
    /// </summary>
    /// <returns>Instance of User object.</returns>
    public User Build()
    {
        return new User
        {
            Name = _name,
            Username = _username,
        };
    }
}
