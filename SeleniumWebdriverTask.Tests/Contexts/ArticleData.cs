// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

namespace SeleniumWebdriverTask.TestLayer.Contexts;

/// <summary>
/// Class to store article title between steps.
/// </summary>
public class ArticleData
{
    /// <summary>
    /// Gets or sets article title.
    /// </summary>
    public required string Title { get; set; }
}
