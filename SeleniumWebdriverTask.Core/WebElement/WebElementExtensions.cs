// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Collections.ObjectModel;
using OpenQA.Selenium;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.CoreLayer.WebElement;

/// <summary>
/// A class with IWebElement related extensions.
/// </summary>
internal static class WebElementExtensions
{
    /// <summary>
    /// Wraps IWebElement into WebElementWrapper.
    /// </summary>
    /// <typeparam name="T">WebElementWrapper type.</typeparam>
    /// <param name="element">Element that needs to be wrapped.</param>
    /// <param name="driverWrapper">DriverWrapper instance.</param>
    /// <returns>Instance of WebElementWrapper.</returns>
    public static T WrapElement<T>(this IWebElement element, DriverWrapper driverWrapper)
        where T : WebElementWrapper
    {
        try
        {
            return (T)Activator.CreateInstance(typeof(T), driverWrapper, element)!;
        }
        catch (MissingMethodException ex)
        {
            throw new InvalidOperationException(
                $"Type {typeof(T).Name} must have a public constructor (IWebDriver, IWebElement).",
                ex);
        }
    }

    /// <summary>
    /// Wraps a collection of IWebElement into collection of WebElementWrapper.
    /// </summary>
    /// <typeparam name="T">WebElementWrapper type.</typeparam>
    /// <param name="elements">Collection that needs to be wrapped.</param>
    /// <param name="driverWrapper">DriverWrapper instance.</param>
    /// <returns>A collection of WebELementWrapper, empty collection if original collection was empty.</returns>
    public static ReadOnlyCollection<T> WrapElements<T>(this ReadOnlyCollection<IWebElement> elements, DriverWrapper driverWrapper)
        where T : WebElementWrapper
    {
        var wrappedElements = new List<T>();
        for (var i = 0; i < elements.Count; i++)
        {
            try
            {
                var element = (T)Activator.CreateInstance(typeof(T), driverWrapper, elements[i])!;
                wrappedElements.Add(element);
            }
            catch (MissingMethodException ex)
            {
                throw new InvalidOperationException(
                    $"Type {typeof(T).Name} must have a public constructor (IWebDriver, IWebElement).",
                    ex);
            }
        }

        return wrappedElements.AsReadOnly();
    }
}
