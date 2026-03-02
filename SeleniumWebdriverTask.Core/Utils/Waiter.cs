// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebdriverTask.CoreLayer.Utils;

/// <summary>
/// A helper class to wait for elements and conditions.
/// </summary>
internal static class Waiter
{
    private const int MaxRetries = 3;

    /// <summary>
    /// Waits until condition becomes true or timeout expires.
    /// </summary>
    /// <param name="wait">WebDriverWait instance.</param>
    /// <param name="condition">Condition to check.</param>
    /// <exception cref="WebDriverTimeoutException">Thrown if condition remained false after wait finished.</exception>
    public static void WaitForCondition(WebDriverWait wait, Func<bool> condition)
    {
        var retries = 0;
        var result = false;
        while (retries < MaxRetries)
        {
            try
            {
                wait.Until(driver =>
                {
                    result = condition.Invoke();
                    return result;
                });
            }
            catch (WebDriverTimeoutException)
            {
                retries++;
            }

            if (result)
            {
                break;
            }
        }

        if (!result)
        {
            throw new WebDriverTimeoutException($"Driver timed out after {MaxRetries} retries.");
        }
    }

    /// <summary>
    /// A wrapper method to allow finding one IWebElement or ReadOnlyCollection of type IWebElement.
    /// </summary>
    /// <typeparam name="T">IWebElement or ReadOnlyCollection of type IWebElement.</typeparam>
    /// <param name="by">Locator to reference in exception.</param>
    /// <param name="findAction">Action to find elements and check if they fit the required condition.</param>
    /// <param name="wait">WebDriverWait instance used to wait.</param>
    /// <returns>One IWebElement or ReadOnlyCollection(IWebElement).</returns>
    /// <exception cref="NoSuchElementException">Thrown if element was not found when looking for a single element.</exception>
    /// <exception cref="StaleElementReferenceException">Thrown if an element or collection were stale.</exception>
    public static T WaitForElements<T>(By by, Func<T> findAction, WebDriverWait wait)
    {
        var retries = 0;
        var exceptionCaught = typeof(NoSuchElementException);
        while (retries < MaxRetries)
        {
            try
            {
                return wait.Until(driver =>
                {
                    try
                    {
                        return findAction.Invoke();
                    }
                    catch (StaleElementReferenceException)
                    {
                        exceptionCaught = typeof(StaleElementReferenceException);
                        return default;
                    }
                    catch (NoSuchElementException)
                    {
                        exceptionCaught = typeof(NoSuchElementException);
                        return default;
                    }
                });
            }
            catch (WebDriverTimeoutException)
            {
                retries++;
            }
        }

        if (exceptionCaught == typeof(NoSuchElementException))
        {
            throw new NoSuchElementException($"Could not find element located by {by} after {MaxRetries} attempts.");
        }
        else
        {
            throw new StaleElementReferenceException($"Element located by {by} remained stale after {MaxRetries} attempts.");
        }
    }
}
